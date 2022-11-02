using System;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Combat;
using Features.Equipment;
using Features.Items;
using Integrations.Actions;
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

            m_EquipmentController.OnHitboxCollided += OnHitboxCollided;

            m_Events = root.GetComponentInChildren<CharacterEvents>();
            
            m_Events.OnStrikeStart += () => DamageEnabled = true;

            m_Events.OnStrikeEnd += () => DamageEnabled = false;

            m_EquipmentController.OnItemEquipped += OnItemEquipped;

            m_CombatController.OnProjectileCollided += OnProjectileCollided;

            m_Character.Events.OnProjectileTrigger += OnProjectileTrigger;
            
            m_CombatController.OnStrike += OnAttemptStrike;
        }

        private void OnProjectileTrigger()
        {
            var position = m_Character.m_EquipmentController.SpawnPositionForSlot("main");

            var itemInSlot = m_Character.m_EquipmentController.ItemInSlot("main");

            if (itemInSlot is not ItemInstance itemInstance) return;

            var payload = FireProjectile.MakePayload(itemInSlot, root.gameObject, gameObject,
                itemInstance.Metadata.RequiredAmmo,
                position, root.forward);

            m_Character.m_ActionsController.DoAction(payload);
        }


        private void OnProjectileCollided(ProjectileCollisionData obj)
        {
            if (obj.Source is not ItemInstance item) return;

            var damagePayload = Damage.MakePayloadForItem(obj.Source, obj.Collider, item);

            obj.Collider.GetComponentInChildren<ActionsController>().DoAction(damagePayload);

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

            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            var totalDamage = 0;

            if (mainSlot is {IsEmpty: false, Main: ItemInstance item})
            {
                totalDamage += item.Metadata.UsageStats["Damage"].Value;
            }

            if (m_Character.Stats)
            {
                totalDamage += m_Character.m_StatsController.CurrentStats["Strength"].Value;
            }

            var damagePayload = Damage.MakePayload(this, target.transform.root.gameObject, totalDamage);

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