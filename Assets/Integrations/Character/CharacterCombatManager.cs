using System;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Combat;
using Features.Equipment;
using Features.WeaponAnimationConfigurations;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private const string DEFAULT_ATTACK_ANIMATION = "Strike_1";

        public Action<DamageTargetActionPayload> OnBeforeDoDamage;

        private Transform Root;

        private Modules.Character m_Character;

        private EquipmentController m_EquipmentController;

        private CombatController m_CombatController;

        private CharacterEvents m_Events;

        private HitboxAnimationController m_HitboxAnimationController;

        private void Awake()
        {
            Root = transform.root;

            m_Character = Root.GetComponent<Modules.Character>();

            m_EquipmentController = Root.GetComponentInChildren<EquipmentController>();

            m_CombatController = Root.GetComponentInChildren<CombatController>();

            m_HitboxAnimationController = Root.GetComponentInChildren<HitboxAnimationController>();
            
            m_HitboxAnimationController.OnAnimationCollision += OnStrikingAnimationCollided;

            m_Events = Root.GetComponentInChildren<CharacterEvents>();
            
            m_EquipmentController.OnItemEquipped += OnItemEquipped;
            
            m_EquipmentController.OnItemUnequipped += OnItemUnequipped;

            m_Character.Events.OnProjectileTrigger += OnProjectileTrigger;
            
            m_CombatController.OnStrike += OnStrike;
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


        private void OnProjectileCollided(ProjectileCollisionData obj)
        {
            if (obj.Source is not ItemInstance item) return;

            if (obj.OriginalCollider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                obj.SetProjectileConsumed();
                
                return;
            }
            
            var damagePayload = DamageTarget.MakePayloadForItem(obj.ProjectileParent, obj.ColliderRoot, item);

            obj.ProjectileParent.GetComponentInChildren<ActionsController>().DoPassiveAction(damagePayload);

            obj.SetProjectileConsumed();
        }

        private void OnItemEquipped(EquipResult obj)
        {
            if (obj.EquippedItem is not ItemInstance item) return;

            if (!item.Metadata.ProvidedAmmo) return;

            var ammo = item.Metadata.ProvidedAmmo.GetComponent<ProjectileController>();

            m_CombatController.SetAmmo(item.Metadata.RequiredAmmo, ammo);
        }

        private void OnStrike()
        {
            var animationName = GetAttackAnimation();

            var configuration = GetAnimationConfiguration();
            
            m_Events.OnStrike?.Invoke(animationName);

            if (configuration == null) return;
            
            m_HitboxAnimationController.Play(configuration.Animation);
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

        private WeaponAnimationDTO GetAnimationConfiguration()
        {
            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            if (mainSlot == null ||
                mainSlot.IsEmpty ||
                mainSlot.Main is not ItemInstance item) return null;

            var animationConfiguration = item.Metadata.WeaponAnimations?.Animations
                .FirstOrDefault(x => x.AnimationType == "main");

            return animationConfiguration;
        }
    }
}