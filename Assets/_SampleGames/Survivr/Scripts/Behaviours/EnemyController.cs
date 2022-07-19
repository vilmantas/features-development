using System;
using System.Collections;
using Features.Health;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;

        private Transform m_Target;

        private bool m_IsExpended = false;
        
        private void Start()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>(); 
            
            m_Target = player.transform;

            m_NavMeshAgent.speed = Random.Range(player.Speed - 3, player.Speed + 1);

            StartCoroutine(FollowTarget());
        }

        public void Damage(CharacterController target)
        {
            if (m_IsExpended) return;
            
            var healthController = target.GetComponentInChildren<HealthController>();

            if (healthController == null) return;

            m_IsExpended = true;
            
            healthController.Damage(3);

            Destroy(gameObject, 0.3f);
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