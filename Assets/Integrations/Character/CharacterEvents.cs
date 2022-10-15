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

        public Action OnStrike;

        public Action OnActivateBlock;

        public Action OnDeactivateBlock;

        public Action OnStrikeStart;

        public Action OnStrikeEnd;

        public bool InputHooksEnabled = true;

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (InputHooksEnabled)
            {
                if (Input.GetMouseButtonDown(2)) OnActivateBlock?.Invoke();
            
                if (Input.GetMouseButtonUp(2)) OnDeactivateBlock?.Invoke();
            
                if (Input.GetKeyDown(KeyCode.LeftShift)) m_NavMeshAgent.speed = 7;

                if (Input.GetKeyUp(KeyCode.LeftShift)) m_NavMeshAgent.speed = 3;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnStrike?.Invoke();
                    m_NavMeshAgent.SetDestination(transform.position);
                }
            }

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