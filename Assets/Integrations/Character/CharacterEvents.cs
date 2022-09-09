using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Character
{
    public class CharacterEvents : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;

        private Vector3 m_PreviousVelocity;

        public Action OnMoving;

        public Action OnStopped;

        public Action OnStrike;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) OnStrike?.Invoke();

            var currentVelocity = m_NavMeshAgent.velocity;

            if (currentVelocity == m_PreviousVelocity) return;

            m_PreviousVelocity = currentVelocity;

            if (currentVelocity == Vector3.zero)
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