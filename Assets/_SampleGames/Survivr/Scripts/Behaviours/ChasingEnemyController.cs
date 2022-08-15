using System.Collections;
using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using Features.Health;
using Features.Health.Events;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace _SampleGames.Survivr
{
    public class ChasingEnemyController : EnemyController
    {
        private HealthController m_Health;

        private bool m_IsExpended;

        private NavMeshAgent m_NavMeshAgent;

        private Transform m_Target;

        private TextMeshPro m_Text;

        private void Awake()
        {
            MeshTransform = transform.Find("model").GetComponentInChildren<MeshRenderer>().transform;

            m_Health = GetComponentInChildren<HealthController>();

            m_Health.OnDeath += HandleDeath;

            m_Health.OnDamage += OnDamage;

            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_Text = GetComponentInChildren<TextMeshPro>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var characterController = colliderRoot.GetComponent<CharacterController>();

            if (characterController == null) return;

            DoDamage(characterController);
        }

        public override void Initialize(int health, CharacterController target)
        {
            m_Health.Initialize(health, health);

            SetHealthText(m_Health.CurrentHealth, m_Health.MaxHealth);

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

        private void DoDamage(CharacterController target)
        {
            if (m_IsExpended) return;

            var actionsController = target.GetComponentInChildren<ActionsController>();

            if (actionsController == null) return;

            var payload = new ActionActivationPayload(new(nameof(Damage)), this, target.transform.root.gameObject);

            actionsController.DoAction(new DamageActionPayload(payload, 5));

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

                m_NavMeshAgent.SetDestination(m_Target.position);

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}