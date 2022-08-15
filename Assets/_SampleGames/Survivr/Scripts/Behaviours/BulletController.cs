using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class BulletController : EnemyController
    {
        public float Speed = 5f;

        private ParticleSystem m_DeathParticles;

        private bool m_IsExpended;

        private Transform m_Mesh;

        private Rigidbody m_Rigidbody;

        private Transform m_Target;

        private void Awake()
        {
            m_Mesh = transform.Find("model");

            m_Rigidbody = GetComponent<Rigidbody>();
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
            m_Target = target.transform;

            var position = m_Target.position;

            var heading = position - transform.position;

            var distance = heading.magnitude;

            var direction = heading / distance;

            m_Rigidbody.AddForce(direction * Speed);

            Destroy(gameObject, 3f);
        }

        private void BeginDestroy()
        {
            m_IsExpended = true;

            Destroy(gameObject);
        }

        private void DoDamage(CharacterController target)
        {
            if (m_IsExpended) return;

            var actionsController = target.GetComponentInChildren<ActionsController>();

            if (actionsController == null) return;

            var payload = new ActionActivationPayload(new(nameof(Damage)), this, target.transform.root.gameObject);

            actionsController.DoAction(new DamageActionPayload(payload, 2));

            BeginDestroy();
        }
    }
}