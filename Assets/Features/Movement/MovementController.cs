using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Movement
{
    public class MovementController : MonoBehaviour
    {
        private GameObject m_RootGameObject;

        private Transform m_RootTransform;
        
        private NavMeshAgent m_Agent;

        public Action<MoveActionData> OnBeforeMove;

        private void Awake()
        {
            m_RootTransform = transform.root;
            
            m_RootGameObject = m_RootTransform.gameObject;

            m_Agent = m_RootGameObject.GetComponent<NavMeshAgent>();

            if (!m_Agent)
            {
                m_Agent = m_RootGameObject.AddComponent<NavMeshAgent>();
                
                m_Agent.speed = 3;
                m_Agent.angularSpeed = 100000;
                m_Agent.acceleration = 5000;
                m_Agent.stoppingDistance = 0.1f;
                m_Agent.autoBraking = false;
            } 
        }

        public void Stop()
        {
            m_Agent.destination = m_RootTransform.position;
        }

        public void MoveToLocation(MoveActionData data)
        {
            OnBeforeMove?.Invoke(data);

            if (data.PreventDefault) return;
            
            m_Agent.destination = data.Destination;
        }

        public void SetRunning(bool running)
        {
            m_Agent.speed = running ? 7 : 3;
        }
    }

    public class MoveActionData
    {
        public readonly Vector3 Destination;

        public bool PreventDefault;

        public MoveActionData(Vector3 destination)
        {
            Destination = destination;
        }
    }
}