using System;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class CharacterController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;
        
        public Action StartedMoving;

        public Action Stopped;

        public float Speed => m_Agent.speed;

        private Vector3 m_PrevVelocity = Vector3.zero;
        
        private void Update()
        {
            var velocity = m_Agent.velocity;
            
            if (velocity != m_PrevVelocity && m_PrevVelocity == Vector3.zero)
            {
                StartedMoving?.Invoke();
            }

            if (velocity == Vector3.zero)
            {
                Stopped?.Invoke();
            }
        }

        private void Awake()
        {
            m_Agent = GetComponentInChildren<NavMeshAgent>();
            
            UserInputManager.OnGroundClicked += OnGroundClicked;
        }

        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var enemyController = colliderRoot.GetComponent<EnemyController>();

            if (enemyController == null) return;
            
            enemyController.Damage(this);
        }

        private void OnGroundClicked(Vector3 point)
        {
            m_Agent.SetDestination(point);
        }
    }
}