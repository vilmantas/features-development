using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Character
{
    public class CharacterEvents : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 Velocity;

        private NavMeshAgent m_NavMeshAgent;

        private Vector3 m_PreviousVelocity;

        public Action OnMoving;

        public Action OnStopped;

        public Action<string> OnStrike;
        
        public Action OnAttemptStrike;

        public Action OnStrikeStart;

        public Action OnStrikeEnd;

        public Action OnProjectileTrigger;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Velocity = m_NavMeshAgent.velocity;

            if (Velocity == m_PreviousVelocity) return;

            m_PreviousVelocity = Velocity;

            if (Velocity == Vector3.zero)
            {
                OnStopped?.Invoke();
            }
            else
            {
                OnMoving?.Invoke();
            }
        }
    }
}