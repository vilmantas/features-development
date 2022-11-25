using System;
using Features.Conditions;
using Integrations.StatusEffects;
using UnityEngine;

namespace Features.OverheadParticles
{
    public class OverheadsController : MonoBehaviour
    {
        private GameObject root;

        private Transform m_HeadAttachmentSpot;

        public GameObject Prefab;
        
        private void Awake()
        {
            root = transform.root.gameObject;

            foreach (Transform VARIABLE in root.GetComponentsInChildren<Transform>())
            {
                if (VARIABLE.name != "Attachment_Head") continue;

                m_HeadAttachmentSpot = VARIABLE;
                
                break;
            }
        }

        private void Start()
        {
            var effects = root.GetComponentInChildren<StatusEffectsController>();
            
            effects.OnAdded += OnAdded;
            
            effects.OnRemoved += OnRemoved;
        }

        private GameObject particle;
        
        private void OnRemoved(StatusEffectMetadata obj)
        {
            if (!particle) return;
            
            Destroy(particle);
        }

        private void OnAdded(StatusEffectMetadata obj)
        {
            if (obj.InternalName != nameof(StunStatusEffect)) return;
            
            particle = Instantiate(Prefab, m_HeadAttachmentSpot);
                
            particle.transform.position += Vector3.up;
        }
    }
}