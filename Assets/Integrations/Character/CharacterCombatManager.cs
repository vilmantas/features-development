using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Combat;
using Features.Conditions;
using Features.Equipment;
using Features.WeaponAnimationConfigurations;
using Integrations.Actions;
using Integrations.Items;
using Integrations.StatusEffects;
using UnityEngine;

namespace Features.Character
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private const string DEFAULT_ATTACK_ANIMATION = "Strike_1";

        private readonly Dictionary<string, Delegate> RunningHandlers = new();

        private readonly ConcurrentDictionary<Guid, Coroutine> RunningRoutines = new();

        private ActionsController m_ActionsController;

        private Modules.Character m_Character;

        private CombatController m_CombatController;

        private EquipmentController m_EquipmentController;

        private CharacterEvents m_Events;

        private HitboxAnimationController m_HitboxAnimationController;

        private StatusEffectsController m_StatusEffectsController;

        public Action<DamageTargetActionPayload> OnBeforeDoDamage;

        public Action OnStrikeInterrupted;

        private Transform Root;

        private void Awake()
        {
            Root = transform.root;

            m_Character = Root.GetComponent<Modules.Character>();

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();

            m_CombatController = Root.GetComponentInChildren<CombatController>();

            m_HitboxAnimationController = Root.GetComponentInChildren<HitboxAnimationController>();

            m_HitboxAnimationController.OnAnimationCollision += OnStrikingAnimationCollided;

            m_HitboxAnimationController.OnHitboxActivated += OnHitboxActivated;

            m_HitboxAnimationController.OnHitboxFinished += OnHitboxFinished;

            m_Events = Root.GetComponentInChildren<CharacterEvents>();

            m_EquipmentController.OnItemEquipped += OnItemEquipped;

            m_EquipmentController.OnItemUnequipped += OnItemUnequipped;

            m_Character.Events.OnProjectileTrigger += OnProjectileTrigger;

            m_CombatController.OnStrike += OnStrike;

            m_StatusEffectsController = Root.GetComponentInChildren<StatusEffectsController>();

            if (m_StatusEffectsController)
            {
                m_StatusEffectsController.OnAdded += OnAdded;
            }

            m_ActionsController = Root.GetComponentInChildren<ActionsController>();
        }

        private void OnStrikingAnimationCollided(Collider currentCollision, List<Collider> allCollisions)
        {
            var target = currentCollision.transform.root.gameObject;

            var count =
                allCollisions.Count(x => x.transform.root.gameObject.name.Equals(target.name));

            if (count > 1) return;

            var damagePayload = DamageTarget.MakePayload(Root.gameObject, target,
                m_Character.m_StatCalculator.GetMainDamage());

            OnBeforeDoDamage?.Invoke(damagePayload);

            m_Character.m_ActionsController.DoAction(damagePayload);
        }

        private void OnItemUnequipped(EquipResult obj)
        {
            if (obj.EquippedItem is not ItemInstance item) return;

            if (!item.Metadata.ProvidedAmmo) return;

            m_CombatController.RemoveAmmo(item.Metadata.RequiredAmmo);
        }

        private void OnProjectileTrigger()
        {
            var position = m_Character.m_EquipmentController.SpawnPositionForSlot("main");

            var itemInSlot = m_Character.m_EquipmentController.ItemInSlot("main");

            if (itemInSlot is not ItemInstance itemInstance) return;

            var payload = FireProjectile.MakePayload(
                itemInSlot,
                Root.gameObject,
                position,
                OnProjectileCollided);

            payload.AmmoType = itemInstance.Metadata.RequiredAmmo;

            payload.Direction = Root.forward;

            m_Character.m_ActionsController.DoPassiveAction(payload);
        }


        private void OnProjectileCollided(ProjectileCollisionData collisionData)
        {
            if (collisionData.Source is not ItemInstance item) return;

            if (collisionData.OriginalCollider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                collisionData.SetProjectileConsumed();

                return;
            }

            var damagePayload = DamageTarget.MakePayloadForItem(collisionData.ProjectileParent, collisionData.ColliderRoot, item);

            collisionData.ProjectileParent.GetComponentInChildren<ActionsController>().DoPassiveAction(damagePayload);

            collisionData.SetProjectileConsumed();
        }

        private void OnItemEquipped(EquipResult obj)
        {
            if (obj.EquippedItem is not ItemInstance item) return;

            if (!item.Metadata.ProvidedAmmo) return;

            var ammo = item.Metadata.ProvidedAmmo.GetComponent<ProjectileController>();

            m_CombatController.SetAmmo(item.Metadata.RequiredAmmo, ammo);
        }

        private void OnAdded(StatusEffectMetadata obj)
        {
            if (!obj.InternalName.Equals(nameof(StunStatusEffect))) return;

            m_HitboxAnimationController.Interrupt();

            var status = new StatusEffectMetadata(nameof(AttackInitiatedStatusEffect));

            var p = new StatusEffectRemovePayload(status);

            m_StatusEffectsController.RemoveStatusEffect(p);

            RunningRoutines.Clear();
        }

        private void OnHitboxFinished()
        {
            RemoveAttackActiveEffect();
        }

        private void RemoveAttackActiveEffect()
        {
            var status = new StatusEffectMetadata(nameof(AttackActiveStatusEffect));

            var p = new StatusEffectRemovePayload(status);

            m_StatusEffectsController.RemoveStatusEffect(p);
        }

        private void OnHitboxActivated()
        {
            RemoveMovementBlocker();

            AddAttackActiveEffect();
        }

        private void AddAttackActiveEffect()
        {
            var status = new StatusEffectMetadata(nameof(AttackActiveStatusEffect));

            var p = new StatusEffectAddPayload(status);

            m_StatusEffectsController.AddStatusEffect(p);
        }

        private void OnStrike()
        {
            var animationName = GetAttackAnimation();

            var configuration = GetAnimationConfiguration();

            m_Events.OnStrike?.Invoke(animationName);

            if (configuration == null) return;

            m_HitboxAnimationController.Play(configuration);

            if (!m_StatusEffectsController) return;

            AddAttackInitiatedEffect();

            var id = Guid.NewGuid();

            var routine = StartCoroutine(StrikeCompletionWaiter(configuration, id));

            RunningRoutines.TryAdd(id, routine);

            StartMovementHandler();
        }

        private void AddAttackInitiatedEffect()
        {
            var status = new StatusEffectMetadata(nameof(AttackInitiatedStatusEffect));

            var p = new StatusEffectAddPayload(status);

            m_StatusEffectsController.AddStatusEffect(p);
        }

        private void StartMovementHandler()
        {
            Action<ActionActivation> handler = MovementChecker;

            m_ActionsController.OnBeforeAction += handler;

            RunningHandlers.Add("movement_blocker", handler);
        }

        private void MovementChecker(ActionActivation obj)
        {
            if (obj.Payload.Action.Name == nameof(Move))
            {
                m_HitboxAnimationController.Interrupt();
                
                RemoveAttackInitiatedEffect();

                RunningRoutines.Clear();

                RemoveMovementBlocker();

                OnStrikeInterrupted?.Invoke();
                
                return;
            }

            obj.PreventDefault = true;
        }

        private void RemoveMovementBlocker()
        {
            if (RunningHandlers.Remove("movement_blocker", out var handler))
            {
                m_ActionsController.OnBeforeAction -= handler as Action<ActionActivation>;
            }
        }

        private IEnumerator StrikeCompletionWaiter(AnimationConfigurationDTO config, Guid Id)
        {
            yield return new WaitForSeconds(config.AnimationDuration);

            if (!RunningRoutines.ContainsKey(Id)) yield break;

            RemoveAttackInitiatedEffect();
        }

        private void RemoveAttackInitiatedEffect()
        {
            var status = new StatusEffectMetadata(nameof(AttackInitiatedStatusEffect));

            var p = new StatusEffectRemovePayload(status);

            m_StatusEffectsController.RemoveStatusEffect(p);
        }

        private string GetAttackAnimation()
        {
            if (!m_EquipmentController) return DEFAULT_ATTACK_ANIMATION;

            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            if (mainSlot == null ||
                mainSlot.IsEmpty ||
                mainSlot.Main is not ItemInstance item) return DEFAULT_ATTACK_ANIMATION;

            var ani = item.Metadata.AttackAnimation;

            if (item.Metadata.WeaponAnimations == null) return ani;

            var animationConfiguration = item.Metadata.WeaponAnimations.Animations
                .FirstOrDefault(x => x.AnimationType == "main");

            if (animationConfiguration == null) return ani;

            ani = animationConfiguration.Animation.AnimationName;

            return ani;
        }

        private AnimationConfigurationDTO GetAnimationConfiguration()
        {
            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            if (mainSlot == null ||
                mainSlot.IsEmpty ||
                mainSlot.Main is not ItemInstance item) return null;

            var animationConfiguration = item.Metadata.WeaponAnimations?.Animations
                .FirstOrDefault(x => x.AnimationType == "main");

            return animationConfiguration?.Animation;
        }
    }
}