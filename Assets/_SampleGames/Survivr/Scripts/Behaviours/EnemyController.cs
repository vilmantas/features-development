using System;
using System.Collections;
using Features.Health;
using Features.Health.Events;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;

        private ParticleSystem m_DeathParticles;

        private Transform m_Target;

        private TextMeshPro m_Text;

        private bool m_IsExpended;

        private Transform m_Mesh;

        private HealthController m_Health;
        
        private void Start()
        {
            SetUpDamageOverTime();

            m_Mesh = transform.Find("mesh");

            m_Health = GetComponentInChildren<HealthController>();
            
            m_Health.OnDeath += HandleDeath;
            
            m_Health.OnDamage += OnDamage;
                
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_DeathParticles = GetComponentInChildren<ParticleSystem>();

            m_Text = GetComponentInChildren<TextMeshPro>();

            var player = FindObjectOfType<PlayerManager>().Player; 
            
            m_Target = player.transform;

            m_NavMeshAgent.speed = Random.Range(player.Speed - 3, player.Speed + 1);

            StartCoroutine(FollowTarget());
        }

        private void SetUpDamageOverTime()
        {
            var dmg = GetComponentInChildren<DamageOverTime>();

            dmg.Damage = 1;

            dmg.Interval = Random.Range(0.5f, 1f);

            dmg.Health = 25;

            dmg.Initialize();
        }

        private void OnDamage(HealthChangeEventArgs obj)
        {
            m_Text.text = obj.After + "/" + obj.Source.MaxHealth;
        }

        private void HandleDeath()
        {
            BeginDestroy();
        }

        private void Damage(CharacterController target)
        {
            if (m_IsExpended) return;
            
            var healthController = target.GetComponentInChildren<HealthController>();

            if (healthController == null) return;

            healthController.Damage(3);

            BeginDestroy();
        }

        private void BeginDestroy()
        {
            m_IsExpended = true;
            
            m_Mesh.gameObject.SetActive(false);

            m_NavMeshAgent.isStopped = true;

            m_Text.enabled = false;
            
            StopAllCoroutines();
            
            m_DeathParticles.Play();
            
            Destroy(gameObject, 6f);
        }

        private IEnumerator FollowTarget()
        {
            while (true)
            {
                m_NavMeshAgent.SetDestination(m_Target.position);

                yield return new WaitForSeconds(0.3f);
            }
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var characterController = colliderRoot.GetComponent<CharacterController>();

            if (characterController == null) return;
            
            Damage(characterController);
        }

    }
}