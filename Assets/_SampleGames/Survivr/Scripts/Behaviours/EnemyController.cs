using System;
using System.Collections;
using Features.Health;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        public float TimeToLive;
        
        private NavMeshAgent m_NavMeshAgent;

        private ParticleSystem m_DeathParticles;

        private Transform m_Target;

        private TextMeshPro m_Text;

        private bool m_IsExpended = false;

        private Transform m_Mesh;
        
        private void Start()
        {
            m_Mesh = transform.Find("mesh");
                
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_DeathParticles = GetComponentInChildren<ParticleSystem>();

            m_Text = GetComponentInChildren<TextMeshPro>();

            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>(); 
            
            m_Target = player.transform;

            m_NavMeshAgent.speed = Random.Range(player.Speed - 3, player.Speed + 1);

            StartCoroutine(FollowTarget());

            StartCoroutine(SelfDestruct(TimeToLive));
        }

        public void Damage(CharacterController target)
        {
            if (m_IsExpended) return;
            
            var healthController = target.GetComponentInChildren<HealthController>();

            if (healthController == null) return;

            m_IsExpended = true;
            
            healthController.Damage(3);

            BeginDestroy();
        }

        private void BeginDestroy()
        {
            m_Mesh.gameObject.SetActive(false);

            m_NavMeshAgent.isStopped = true;
            
            StopAllCoroutines();
            
            m_DeathParticles.Play();
            
            Destroy(gameObject, 6f);
        }

        private IEnumerator SelfDestruct(float delaySeconds)
        {
            while (delaySeconds >= 0)
            {
                delaySeconds -= 0.1f;

                yield return new WaitForSeconds(0.1f);

                m_Text.text = delaySeconds.ToString("0.0");
            }

            yield return new WaitForSeconds(delaySeconds);

            BeginDestroy();
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