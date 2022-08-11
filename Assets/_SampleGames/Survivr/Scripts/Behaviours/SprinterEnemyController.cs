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
    public class SprinterEnemyController : EnemyController
    {
        private NavMeshAgent m_NavMeshAgent;

        private ParticleSystem m_DeathParticles;

        private Transform m_Target;

        private TextMeshPro m_Text;

        private bool m_IsExpended;

        private Transform m_Mesh;

        private HealthController m_Health;
        
        private void Awake()
        {
            m_Mesh = transform.Find("model");

            m_Health = GetComponentInChildren<HealthController>();
            
            m_Health.OnDeath += HandleDeath;
            
            m_Health.OnDamage += OnDamage;
                
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_DeathParticles = GetComponentInChildren<ParticleSystem>();

            m_Text = GetComponentInChildren<TextMeshPro>();
        }
        
        public override void Initialize(int health, CharacterController target)
        {
            m_Health.Initialize(health, health);
            
            SetHealthText(m_Health.CurrentHealth, m_Health.MaxHealth);
            
            m_Target = target.transform;

            m_NavMeshAgent.speed = Random.Range(target.Speed + 10, target.Speed + 20);

            StartCoroutine(FollowTarget());
        }
        
        private IEnumerator FollowTarget()
        {
            var navMesh = m_Target.root.GetComponent<NavMeshAgent>();
            
            while (true)
            {
                m_NavMeshAgent.SetDestination(navMesh.destination);

                if (m_IsExpended) break;
                
                yield return new WaitForSeconds(3);
            }
        }
        
        private void OnDamage(HealthChangeEventArgs obj)
        {
            SetHealthText(obj.After, obj.Source.MaxHealth);
        }

        private void SetHealthText(int after, int max)
        {
            m_Text.text = after + "/" + max;
        }
        
        private void HandleDeath()
        {
            BeginDestroy();
        }
        
        private void BeginDestroy()
        {
            m_IsExpended = true;
            
            m_Mesh.gameObject.SetActive(false);

            m_NavMeshAgent.isStopped = true;

            m_Text.enabled = false;
            
            StopAllCoroutines();
            
            // m_DeathParticles.Play();
            
            Destroy(gameObject, 6f);
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var characterController = colliderRoot.GetComponent<CharacterController>();

            if (characterController == null) return;
            
            Damage(characterController);
        }
        
        private void Damage(CharacterController target)
        {
            if (m_IsExpended) return;
            
            var healthController = target.GetComponentInChildren<HealthController>();

            if (healthController == null) return;

            healthController.Damage(3);

            BeginDestroy();
        }
    }
}