using System;
using Features.Actions;
using Features.Conditions;
using Features.Health;
using Integrations.Actions;
using Integrations.StatusEffects;
using UnityEngine;

namespace Features.Character
{
    public class CharacterExpirationManager : MonoBehaviour
    {
        private GameObject Root;

        private HealthController m_HealthController;

        private CharacterEvents m_CharacterEvents;

        private ActionsController m_ActionsController;

        private StatusEffectsController m_StatusEffectsController;
        
        private void Awake()
        {
            Root = transform.root.gameObject;
        }

        private void Start()
        {
            m_ActionsController = Root.GetComponentInChildren<ActionsController>();
            
            m_CharacterEvents = Root.GetComponentInChildren<CharacterEvents>();

            m_HealthController = Root.GetComponentInChildren<HealthController>();

            m_StatusEffectsController = Root.GetComponentInChildren<StatusEffectsController>();
            
            if (!m_HealthController) return;
            
            m_HealthController.OnDepleted += AnnounceExpiration;
        }
        
        private void AnnounceExpiration()
        {
            if (m_StatusEffectsController &&
                m_StatusEffectsController.IsAffectedBy(nameof(DeathStatusEffect))) return;

            var payload = Die.MakePayload(Root, Root, "Health at zero.");

            m_ActionsController.DoPassiveAction(payload);
            
            m_CharacterEvents.OnDeath?.Invoke();
        }
    }
}