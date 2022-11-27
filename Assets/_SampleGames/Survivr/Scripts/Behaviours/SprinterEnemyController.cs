using System.Collections;
using _SampleGames.Survivr.SurvivrFeatures.Combat;
using Features.Actions;
using Features.Health;
using Features.Health.Events;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class SprinterEnemyController : EnemyController
    {
        private bool m_IsExpended;

        private NavMeshAgent m_NavMeshAgent;

        private Transform m_Target;

        private TextMeshPro m_Text;

        private void Awake()
        {
            MeshTransform = transform.Find("model").GetComponentInChildren<MeshRenderer>().transform;

            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_Text = GetComponentInChildren<TextMeshPro>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var characterController = colliderRoot.GetComponent<CharacterController>();

            if (characterController == null) return;

            Damage(characterController);
        }

        protected override void OnInitialize(int health, CharacterController target)
        {
            HealthController.OnDepleted += HandleDeath;

            HealthController.OnDamage += OnDamage;
            
            HealthController.Initialize(health, health);

            SetHealthText(HealthController.CurrentHealth, HealthController.MaxHealth);

            m_Target = target.transform;

            m_NavMeshAgent.speed = Random.Range(target.Speed + 10, target.Speed + 20);

            StartCoroutine(FollowTarget());
        }

        private IEnumerator FollowTarget()
        {
            var navMesh = m_Target.root.GetComponent<NavMeshAgent>();

            while (true)
            {
                if (m_IsExpended) break;

                m_NavMeshAgent.SetDestination(navMesh.destination);

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

        // protected override void AttackResultCallback(AttackResult result)
        // {
        //     if (result.HitMetadataBase.DamageDealt < 1) return;
        //     
        //     BeginDestroy();
        // }
        
        private void BeginDestroy()
        {
            DestroyWithParticles();
        }
    }
}