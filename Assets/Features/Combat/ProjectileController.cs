using System;
using UnityEngine;

namespace Features.Combat
{
    public class ProjectileController : MonoBehaviour
    {
        public Action<ProjectileCollisionData> OnProjectileCollided;
        
        private GameObject m_Parent;

        public void Initialize(GameObject parent, Transform direction)
        {
            m_Parent = parent;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "hitbox") return;

            var data = new ProjectileCollisionData(m_Parent, other.gameObject);
            
            OnProjectileCollided?.Invoke(data);

            if (!data.IsConsumed) return;
            
            Destroy(gameObject);
        }
    }

    public class ProjectileCollisionData
    {
        public readonly GameObject ProjectileParent;

        public readonly GameObject Collider;

        private bool m_isConsumed;
        
        public bool IsConsumed => m_isConsumed;

        public ProjectileCollisionData(GameObject parent, GameObject collider)
        {
            Collider = collider;
            ProjectileParent = parent;
        }

        public void SetProjectileConsumed()
        {
            m_isConsumed = true;
        }
    }
}