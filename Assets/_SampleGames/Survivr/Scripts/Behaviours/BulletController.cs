using _SampleGames.Survivr.SurvivrFeatures.Actions;
using Features.Actions;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class BulletController : MonoBehaviour
    {
        public float Speed = 5f;

        private bool m_IsExpended;

        private Rigidbody m_Rigidbody;

        private GameObject m_Source;

        private Transform m_Target;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            var colliderRoot = collision.transform.root;

            var characterController = colliderRoot.GetComponent<CharacterController>();

            if (characterController == null) return;

            DoDamage(characterController);
        }

        public void Initialize(CharacterController target, GameObject source)
        {
            m_Source = source;

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