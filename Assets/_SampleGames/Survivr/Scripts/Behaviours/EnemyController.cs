using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;

        private Transform m_Target;
        
        private void Start()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>(); 
            
            m_Target = player.transform;

            m_NavMeshAgent.speed = Random.Range(player.Speed - 3, player.Speed + 1);

            StartCoroutine(FollowTarget());
        }

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.collider.name);
        }

        private IEnumerator FollowTarget()
        {
            while (true)
            {
                m_NavMeshAgent.SetDestination(m_Target.position);

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}