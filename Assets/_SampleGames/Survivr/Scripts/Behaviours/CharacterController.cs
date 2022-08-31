using System;
using Features.Health;
using Features.Health.Events;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Items;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class CharacterController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;

        private HealthController m_HealthController;

        private Vector3 m_PrevVelocity = Vector3.zero;

        public Action OnDeath;

        public Action OnStartedMoving;

        public Action OnStopped;

        public float Speed => m_Agent.speed;

        private void Awake()
        {
            m_Agent = GetComponentInChildren<NavMeshAgent>();

            UserInputManager.OnGroundClicked += OnGroundClicked;

            m_HealthController = GetComponentInChildren<HealthController>();

            m_HealthController.OnChange += HandleChange;
        }

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

        private void OnDestroy()
        {
            UserInputManager.OnGroundClicked -= OnGroundClicked;

            m_HealthController.OnChange -= HandleChange;
        }

        private void HandleChange(HealthChangeEventArgs args)
        {
            if (args.After > 0) return;

            OnDeath?.Invoke();
        }

        private void OnGroundClicked(Vector3 point)
        {
            m_Agent.SetDestination(point);
        }
    }
}