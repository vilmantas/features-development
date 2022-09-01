using System.Collections;
using _SampleGames.Survivr.SurvivrFeatures.Actions;
using _SampleGames.Survivr.SurvivrFeatures.Combat;
using Features.Actions;
using Features.Health;
using Features.Health.Events;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class ShooterEnemyController : EnemyController
    {
        public GameObject BulletPrefab;

        private bool m_IsExpended;

        private NavMeshAgent m_NavMeshAgent;

        private float m_ShootingDelay = 5f;

        private CharacterController m_Target;

        private Transform m_TargetTransform;

        private TextMeshPro m_Text;

        private float m_TimeSinceLastShot = 0f;

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
            HealthController.OnDeath += HandleDeath;

            HealthController.OnDamage += OnDamage;
            
            HealthController.Initialize(health, health);

            SetHealthText(HealthController.CurrentHealth, HealthController.MaxHealth);

            m_Target = target;

            m_TargetTransform = target.transform;

            m_NavMeshAgent.speed = Random.Range(target.Speed - 3, target.Speed + 1);

            StartCoroutine(FollowTarget());

            StartCoroutine(ShootingChecker());

            StartCoroutine(ShootingTicker());
        }

        private IEnumerator ShootingTicker()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                m_TimeSinceLastShot += 0.1f;
            }
        }

        private IEnumerator ShootingChecker()
        {
            while (true)
            {
                if (m_IsExpended) break;

                if (m_NavMeshAgent.velocity != Vector3.zero)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                if (m_TimeSinceLastShot <= m_ShootingDelay)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                m_TimeSinceLastShot = 0f;

                var bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

                bullet.GetComponent<BulletController>().Initialize(m_Target, transform.root.gameObject);

                yield return new WaitForSeconds(0.1f);
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
            DestroyWithParticles();
        }

        private IEnumerator FollowTarget()
        {
            while (true)
            {
                if (m_IsExpended) break;

                m_NavMeshAgent.SetDestination(m_TargetTransform.position);

                yield return new WaitForSeconds(0.3f);
            }
        }
        
        protected override void AttackResultCallback(AttackResult result)
        {
            if (result.HitMetadataBase.DamageDealt < 1) return;
            
            BeginDestroy();
        }
    }
}