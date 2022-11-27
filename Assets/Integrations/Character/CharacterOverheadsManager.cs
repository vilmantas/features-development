using System;
using Features.Conditions;
using Features.OverheadParticles;
using UnityEngine;

namespace Features.Character
{
    public class CharacterOverheadsManager : MonoBehaviour
    {
        private GameObject Root;

        private OverheadsController m_OverheadsController;

        private StatusEffectsController m_StatusEffectsController;
        
        private void Awake()
        {
            Root = transform.root.gameObject;
        }

        private void Start()
        {
            m_OverheadsController = Root.GetComponentInChildren<OverheadsController>();
            
            m_StatusEffectsController = Root.GetComponentInChildren<StatusEffectsController>();

            if (!m_StatusEffectsController) return;
            
            m_StatusEffectsController.OnAdded += OnAdded;
            
            m_StatusEffectsController.OnRemoved += OnRemoved;
        }

        private void OnAdded(StatusEffectMetadata obj)
        {
            m_OverheadsController.AddOverhead(obj.InternalName);
        }
        
        private void OnRemoved(StatusEffectMetadata obj)
        {
            m_OverheadsController.RemoveOverhead(obj.InternalName);
        }
    }
}