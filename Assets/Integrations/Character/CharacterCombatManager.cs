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

        private Transform root;

        private EquipmentController m_EquipmentController;

        private CharacterEvents m_Events;

        private bool DamageEnabled;
        
        private void Awake()
        {
            root = transform.root;

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnHitboxCollided += OnHitboxCollided;

            m_Events = root.GetComponentInChildren<CharacterEvents>();

            m_Events.OnAttemptStrike += () => m_Events.OnStrike?.Invoke(GetAttackAnimation());

            m_Events.OnStrikeStart += () => DamageEnabled = true;

            m_Events.OnStrikeEnd += () => DamageEnabled = false;
        }

        private void OnHitboxCollided(string arg1, Collider arg2)
        {
            if (!DamageEnabled) return;

            var c = arg2.transform.root.GetComponent<CharacterC.Character>();

            if (!c) return;

            var mainSlot =
                m_EquipmentController.ContainerSlots.FirstOrDefault(x =>
                    x.Slot.ToLower() == "main");

            var damage = 1;

            if (mainSlot != null &&
                mainSlot.IsEmpty &&
                mainSlot.Main is ItemInstance item)
            {
                damage = item.Metadata.UsageStats["Damage"]?.Value ?? damage;
            }
            
            var damagePayload = Damage.MakePayload(this, c.gameObject, damage);

            c.m_ActionsController.DoAction(damagePayload);
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