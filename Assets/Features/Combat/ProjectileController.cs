using System;
using UnityEngine;

namespace Features.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileController : MonoBehaviour
    {
        public Action<ProjectileCollisionData> OnProjectileCollided;
        
        private GameObject m_Parent;

        private Rigidbody m_Rigidbody;

        private Vector3 m_Direction;

        public void Initialize(GameObject parent, Vector3 direction)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            
            m_Parent = parent;

            m_Direction = direction;
            
            m_Rigidbody.AddForce(m_Direction * 600);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.name == m_Parent.name) return;
            
            if (other.name != "hitbox") return;
            
            print("Projectile hit " + other.transform.root.name);

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