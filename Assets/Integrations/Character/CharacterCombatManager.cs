using System;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Combat;
using Features.Equipment;
using Integrations.Actions;
using Integrations.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private const string DEFAULT_ATTACK_ANIMATION = "Strike_1";

        public Action<DamageActionPayload> OnBeforeDoDamage;

        private Transform root;

        private Modules.Character m_Character;

        private EquipmentController m_EquipmentController;

        private CombatController m_CombatController;

        private CharacterEvents m_Events;

        private bool DamageEnabled;

        private void Awake()
        {
            root = transform.root;

            m_Character = root.GetComponent<Modules.Character>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_CombatController = root.GetComponentInChildren<CombatController>();
            
            m_CharacterStatCalculator = root.GetComponentInChildren<CharacterStatCalculator>();

            m_EquipmentController.OnHitboxCollided += OnHitboxCollided;

            m_Events = root.GetComponentInChildren<CharacterEvents>();
            
            m_Events.OnStrikeStart += () => DamageEnabled = true;

            m_Events.OnStrikeEnd += () => DamageEnabled = false;

            m_EquipmentController.OnItemEquipped += OnItemEquipped;
            
            m_EquipmentController.OnItemUnequipped += OnItemUnequipped;

            m_CombatController.OnProjectileCollided += OnProjectileCollided;

            m_Character.Events.OnProjectileTrigger += OnProjectileTrigger;
            
            m_CombatController.OnStrike += OnAttemptStrike;
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

            var payload = FireProjectile.MakePayload(itemInSlot, root.gameObject, null,
                itemInstance.Metadata.RequiredAmmo,
                position, root.forward);

            m_Character.m_ActionsController.DoAction(payload);
        }


        private void OnProjectileCollided(ProjectileCollisionData obj)
        {
            if (obj.Source is not ItemInstance item) return;
            
            var damagePayload = Damage.MakePayloadForItem(obj.ProjectileParent, obj.Collider, item);

            damagePayload.Data.Add("passive", true);
            
            obj.ProjectileParent.GetComponentInChildren<ActionsController>().DoAction(damagePayload);

            obj.SetProjectileConsumed();
        }

        private void OnItemEquipped(EquipResult obj)
        {
            if (obj.EquippedItem is not ItemInstance item) return;

            if (!item.Metadata.ProvidedAmmo) return;

            var ammo = item.Metadata.ProvidedAmmo.GetComponent<ProjectileController>();

            m_CombatController.SetAmmo(item.Metadata.RequiredAmmo, ammo);
        }

        private void OnAttemptStrike()
        {
            m_Events.OnStrike?.Invoke(GetAttackAnimation());
        }

        private void OnHitboxCollided(string slot, Collider target)
        {
            if (!DamageEnabled) return;

            var damagePayload = Damage.MakePayload(root.gameObject, target.transform.root.gameObject,
                m_Character.m_StatCalculator.GetMainDamage());

            OnBeforeDoDamage?.Invoke(damagePayload);

            m_Character.m_ActionsController.DoAction(damagePayload);
        }

        public string GetAttackAnimation()
        {
            if (!m_EquipmentController) return DEFAULT_ATTACK_ANIMATION;

            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            if (mainSlot == null ||
                mainSlot.IsEmpty ||
                mainSlot.Main is not ItemInstance item) return DEFAULT_ATTACK_ANIMATION;

            return item.Metadata.AttackAnimation;
        }
    }
}