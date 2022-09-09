using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Character
{
    public class CharacterEvents : MonoBehaviour
    {
        public Vector3 Velocity;

        public float Speed;
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
            if (Input.GetKeyDown(KeyCode.LeftShift)) m_NavMeshAgent.speed = 7;

            if (Input.GetKeyUp(KeyCode.LeftShift)) m_NavMeshAgent.speed = 3;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnStrike?.Invoke();
                m_NavMeshAgent.SetDestination(transform.position);
            }

            Velocity = m_NavMeshAgent.velocity;

            Speed = m_NavMeshAgent.speed;

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