using System;
using System.Linq;
using Features.Equipment;
using Features.Items;
using UnityEngine;

namespace Features.Character
{
    public class CharacterCombatManager : MonoBehaviour
    {
        private const string DEFAULT_ATTACK_ANIMATION = "Strike_1";

        private Transform root;

        private EquipmentController m_EquipmentController;

        private CharacterEvents m_Events;
        
        private void Awake()
        {
            root = transform.root;

            m_EquipmentController = root.GetComponentInChildren<EquipmentController>();

            m_Events = root.GetComponentInChildren<CharacterEvents>();

            m_Events.OnAttemptStrike += () => m_Events.OnStrike?.Invoke(GetAttackAnimation());
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