using System;
using System.Linq;
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

        private CharacterEvents m_Events;

        private bool DamageEnabled;

        private void Awake()
        {
            root = transform.root;

            m_Character = root.GetComponent<Modules.Character>();

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnHitboxCollided += OnHitboxCollided;

            m_Events = root.GetComponentInChildren<CharacterEvents>();

            m_Events.OnAttemptStrike += OnAttemptStrike;

            m_Events.OnStrikeStart += () => DamageEnabled = true;

            m_Events.OnStrikeEnd += () => DamageEnabled = false;
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
            
            var damagePayload = Damage.MakePayload(this, target.gameObject, totalDamage);

            OnBeforeDoDamage?.Invoke(damagePayload);

            m_Character.m_ActionsController.DoAction(damagePayload);
        }

        public string GetAttackAnimation()
        {
            if (!m_EquipmentController) return DEFAULT_ATTACK_ANIMATION;
            
            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x => x.Slot.ToLower() == "main");

            if (mainSlot == null || 
                mainSlot.IsEmpty || 
                mainSlot.Main is not ItemInstance item) return DEFAULT_ATTACK_ANIMATION;

            return item.Metadata.AttackAnimation;
        }
    }
}