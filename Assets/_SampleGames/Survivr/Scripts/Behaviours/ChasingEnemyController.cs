using System.Collections;
using _SampleGames.Survivr.SurvivrFeatures.Combat;
using Features.Actions;
using Features.Combat;
using Features.Health;
using Features.Health.Events;
using Features.Stats.Base;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class ChasingEnemyController : EnemyController
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

            m_NavMeshAgent.speed = Random.Range(target.Speed - 3, target.Speed + 1);

            StartCoroutine(FollowTarget());
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

        private IEnumerator FollowTarget()
        {
            while (true)
            {
                if (m_IsExpended) break;

                m_NavMeshAgent.SetDestination(m_Target.position);

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}