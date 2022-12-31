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

        private GameObject m_Target;

        private Rigidbody m_Rigidbody;

        private Vector3 m_Direction;

        private void Update()
        {
            if (!m_Target) return;
            
            transform.LookAt(m_Target.transform.position);
        }

        public void Initialize(GameObject parent, object source , Vector3 direction, GameObject target)
        {
            m_Source = source;
            
            m_Rigidbody = GetComponent<Rigidbody>();
            
            m_Parent = parent;

            m_Target = target;

            m_Direction = direction;
            
            m_Rigidbody.AddForce(m_Direction * 600);
            
            Destroy(gameObject, 5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_Parent == null) return;
            
            if (other.transform.root.name == m_Parent.name) return;
            
            var data = new ProjectileCollisionData(this, m_Parent, other.transform.root.gameObject, m_Source, other);
            
            OnProjectileCollided?.Invoke(data);
            
            if (!data.IsConsumed) return;
            
            OnProjectileCollided = null;
            
            Destroy(gameObject);
        }
    }
}