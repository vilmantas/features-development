using System;
using Features.Combat;
using Features.Conditions;
using Integrations.Character.StatusEffects;
using UnityEngine;

namespace Features.Character
{
    public class CharacterStatusEffectsManager : MonoBehaviour
    {
        private StatusEffectsController m_StatusEffectsController;

        private CombatController m_CombatController;

        private CharacterEvents m_Events;

        private Transform root;
        
        private void Awake()
        {
            root = transform.root;

            m_CombatController = root.GetComponentInChildren<CombatController>();
            
            m_Events = root.GetComponentInChildren<CharacterEvents>();
            
            m_Events.OnStrikeStart += AddActionStatusEffect;
            
            m_Events.OnStrikeEnd += RemoveActionStatusEffect;
        }

        private void RemoveActionStatusEffect()
        {
            var controller = root.GetComponentInChildren<StatusEffectsController>();

            var status = new StatusEffectMetadata(nameof(DoingActionStatusEffect));

            var payload = new StatusEffectRemovePayload(status);
        
            controller.RemoveStatusEffect(payload);
        }

        private void AddActionStatusEffect()
        {
            var controller = root.GetComponentInChildren<StatusEffectsController>();

            var status = new StatusEffectMetadata(nameof(DoingActionStatusEffect));

            var payload = new StatusEffectAddPayload(status);
        
            controller.AddStatusEffect(payload);
        }
    }
}