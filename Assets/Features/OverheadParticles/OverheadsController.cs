using System.Collections.Generic;
using UnityEngine;

namespace Features.OverheadParticles
{
    public class OverheadsController : MonoBehaviour
    {
        private GameObject root;

        private Transform m_HeadAttachmentSpot;

        private Dictionary<string, ParticleSystem> m_PlayingParticles = new();
        
        private void Awake()
        {
            root = transform.root.gameObject;
        }

        private void Start()
        {
            foreach (Transform VARIABLE in root.GetComponentsInChildren<Transform>())
            {
                if (VARIABLE.name != "Attachment_Head") continue;

                m_HeadAttachmentSpot = VARIABLE;
                
                break;
            }
        }

        
        public void RemoveOverhead(string effectName)
        {
            if (!m_PlayingParticles.TryGetValue(effectName, out var particles)) return;

            m_PlayingParticles.Remove(effectName);
            
            Destroy(particles);
        }

        public void AddOverhead(string effectName)
        {
            if (!OverheadsRegistry.Implementations.TryGetValue(effectName,
                    out var stuff))
            {
                return;
            }
            
            if (m_PlayingParticles.ContainsKey(effectName)) return;
            
            var particles = Instantiate(stuff, m_HeadAttachmentSpot);
                
            particles.transform.position += new Vector3(0, 0.5f, 0);
            
            m_PlayingParticles.Add(effectName, particles);
        }
    }
}