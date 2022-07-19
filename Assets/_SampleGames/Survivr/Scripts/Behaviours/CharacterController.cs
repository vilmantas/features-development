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

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.collider.transform.root.name);
        }

        private void OnGroundClicked(Vector3 point)
        {
            m_Agent.SetDestination(point);
        }
    }
}