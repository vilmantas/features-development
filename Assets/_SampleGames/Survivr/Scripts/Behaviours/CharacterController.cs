using System;
using Features.Health;
using Features.Health.Events;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class CharacterController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;

        private HealthController m_HealthController;
        
        public Action OnStartedMoving;

        public Action OnStopped;

        public Action OnDeath;

        public float Speed => m_Agent.speed;

        private Vector3 m_PrevVelocity = Vector3.zero;
        
        private void Update()
        {
            var velocity = m_Agent.velocity;
            
            if (velocity != m_PrevVelocity && m_PrevVelocity == Vector3.zero)
            {
                OnStartedMoving?.Invoke();
            }

            if (velocity == Vector3.zero)
            {
                OnStopped?.Invoke();
            }
        }

        private void Awake()
        {
            m_Agent = GetComponentInChildren<NavMeshAgent>();
            
            UserInputManager.OnGroundClicked += OnGroundClicked;

            m_HealthController = GetComponentInChildren<HealthController>();

            m_HealthController.OnChange += HandleChange();
        }

        private Action<HealthChangeEventArgs> HandleChange()
        {
            return args =>
            {
                if (args.After > 0) return;
                
                OnDeath?.Invoke();
            };
        }
        
        private void OnGroundClicked(Vector3 point)
        {
            m_Agent.SetDestination(point);
        }

        private void OnDestroy()
        {
            UserInputManager.OnGroundClicked -= OnGroundClicked;
            
            m_HealthController.OnChange -= HandleChange();
        }
    }
}