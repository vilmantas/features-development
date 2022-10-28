using System;
using UnityEngine;

namespace Features.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileController : MonoBehaviour
    {
        public Action<ProjectileCollisionData> OnProjectileCollided;

        private object m_Source;
        
        private GameObject m_Parent;

        private Rigidbody m_Rigidbody;

        private Vector3 m_Direction;

        public void Initialize(GameObject parent, object source , Vector3 direction)
        {
            m_Source = source;
            
            m_Rigidbody = GetComponent<Rigidbody>();
            
            m_Parent = parent;

            m_Direction = direction;
            
            m_Rigidbody.AddForce(m_Direction * 600);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.name == m_Parent.name) return;
            
            if (other.name != "hitbox") return;
            
            var data = new ProjectileCollisionData(m_Parent, other.transform.root.gameObject, m_Source);
            
            OnProjectileCollided?.Invoke(data);

            if (!data.IsConsumed) return;

            OnProjectileCollided = null;
            
            Destroy(gameObject);
        }
    }

    public class ProjectileCollisionData
    {
        public readonly GameObject ProjectileParent;

        public readonly GameObject Collider;

        public readonly object Source;

        private bool m_isConsumed;
        
        public bool IsConsumed => m_isConsumed;

        public ProjectileCollisionData(GameObject parent, GameObject collider, object source)
        {
            Collider = collider;
            Source = source;
            ProjectileParent = parent;
        }

        public void SetProjectileConsumed()
        {
            m_isConsumed = true;
        }
    }
}