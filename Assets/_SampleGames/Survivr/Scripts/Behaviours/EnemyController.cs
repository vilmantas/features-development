using System.Collections;
using _SampleGames.Survivr.SurvivrFeatures.Actions;
using _SampleGames.Survivr.SurvivrFeatures.Combat;
using Features.Actions;
using Features.Combat;
using Features.Health;
using Features.Stats.Base;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        private StatsController StatsController;

        protected HealthController HealthController;
        
        public GameObject DeathParticles;

        internal Transform MeshTransform;
        
        public void Initialize(int health, CharacterController target)
        {
            HealthController = GetComponentInChildren<HealthController>();
            
            StatsController = transform.root.GetComponentInChildren<StatsController>();
            
            OnInitialize(health, target);
        }

        protected virtual void OnInitialize(int health, CharacterController target)
        {
            
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        internal void DestroyWithParticles()
        {
            StartCoroutine(DestroyWithParticlesInner());
        }

        private IEnumerator DestroyWithParticlesInner()
        {
            yield return null;

            if (DeathParticles != null)
            {
                var particles = Instantiate(DeathParticles, MeshTransform.position, Quaternion.identity);

                Destroy(particles, 5f);
            }

            Destroy(gameObject);
        }

        protected void Damage(CharacterController target)
        {
            var actionsController = target.GetComponentInChildren<ActionsController>();

            var defendPayload = DefendActionPayload(target);

            actionsController.DoAction(defendPayload);
        }

        private DefendActionPayload DefendActionPayload(CharacterController target)
        {
            var payload =
                new ActionActivationPayload(new(nameof(Defend)), this, target.transform.root.gameObject);

            var defendPayload = new DefendActionPayload(payload,
                new AttackData(transform.root.gameObject, null, CalculateDamage()),
                AttackResultCallback
            );
            return defendPayload;
        }

        protected virtual void AttackResultCallback(AttackResult result)
        {
            
        }

        protected virtual int CalculateDamage()
        {
            return StatsController.CurrentStats["Strength"].Value;
        }
    }
}