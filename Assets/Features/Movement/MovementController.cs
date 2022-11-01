using System;
using UnityEngine;
using UnityEngine.AI;

namespace Features.Movement
{
    public class MovementController : MonoBehaviour
    {
        private GameObject m_root;
        
        private NavMeshAgent m_Agent;
        
        private void Awake()
        {
            m_root = transform.root.gameObject;

            m_Agent = m_root.GetComponent<NavMeshAgent>();

            if (!m_Agent)
            {
                m_Agent = m_root.AddComponent<NavMeshAgent>();
                
                m_Agent.speed = 3;
                m_Agent.angularSpeed = 100000;
                m_Agent.acceleration = 5000;
                m_Agent.stoppingDistance = 0.1f;
                m_Agent.autoBraking = false;
            } 
        }

        public void MoveToLocation(Vector3 dest)
        {
            m_Agent.destination = dest;
        }

        public void SetRunning(bool running)
        {
            m_Agent.speed = running ? 7 : 3;
        }
    }
}