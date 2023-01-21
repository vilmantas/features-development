using System;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class HitboxPlayer : MonoBehaviour
    {
        public Action<Collider> OnCollision;

        private bool m_Initialized;

        private Transform m_Parent;
        
        public void Initialize()
        {
            m_Parent = transform.root.transform;
            
            m_Initialized = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!m_Initialized) return;
            
            if (other.transform.root.name == m_Parent.name) return;
            
            OnCollision?.Invoke(other);
        }

        private void OnDestroy()
        {
            OnCollision = null;
        }
    }
}