using System;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class CharacterController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;

        public float Speed => m_Agent.speed;
        
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