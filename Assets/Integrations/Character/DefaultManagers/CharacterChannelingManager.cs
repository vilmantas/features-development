using System;
using System.Collections.Generic;
using System.Linq;
using Features.Conditions;
using Features.Cooldowns;
using Features.Skills;
using Integrations.StatusEffects;
using UnityEngine;

namespace Features.Character
{
    public class CharacterChannelingManager : MonoBehaviour
    {
        private GameObject Root;

        private ChannelingController m_ChannelingController;

        private StatusEffectsController m_StatusEffectsController;

        private CharacterEvents m_Events;
        
        private void Start()
        {
            Root = transform.root.gameObject;

            m_ChannelingController = Root.GetComponentInChildren<ChannelingController>();
            
            m_ChannelingController.OnChannelingStarted += OnChannelingStarted;
            
            m_ChannelingController.OnChannelingCompleted += OnChannelingCompleted;
            
            m_StatusEffectsController = Root.GetComponentInChildren<StatusEffectsController>();

            m_Events = Root.GetComponentInChildren<CharacterEvents>();
        }

        private void OnChannelingCompleted(ChannelingItem obj)
        {
            var status = new StatusEffectMetadata(nameof(ChannelingStatusEffect));

            var p = new StatusEffectRemovePayload(status);
        
            m_StatusEffectsController.RemoveStatusEffect(p);
            
            m_Events.OnChannelingEnd?.Invoke();
        }

        private void OnChannelingStarted(ChannelingItem obj)
        {
            var status = new StatusEffectMetadata(nameof(ChannelingStatusEffect));

            var p = new StatusEffectAddPayload(status);
        
            m_StatusEffectsController.AddStatusEffect(p);
            
            m_Events.OnChannelingStart?.Invoke("Lmao");
        }
    }
}