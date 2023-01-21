using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Features.WeaponAnimationConfigurations
{
    public class HitboxPlayer : MonoBehaviour
    {
        public Guid Id { get; private set; }
        
        public Action<Collider> OnCollision;

        public Action OnDestruction;

        private bool m_Initialized;

        private Transform m_Parent;

        public ConcurrentStack<Collider> Collisions = new();

        private void Awake()
        {
            Id = Guid.NewGuid();
        }

        public void Initialize()
        {
            m_Parent = transform.root.transform;
            
            m_Initialized = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!m_Initialized) return;
            
            if (other.transform.root.name == m_Parent.name) return;
            
            Collisions.Push(other);
            
            OnCollision?.Invoke(other);
        }

        private void OnDestroy()
        {
            OnDestruction?.Invoke();
            
            OnDestruction = null;
            OnCollision = null;
        }
    }
}