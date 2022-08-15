using System.Collections;
using UnityEngine;

namespace _SampleGames.Survivr
{
    public class EnemyController : MonoBehaviour
    {
        public GameObject DeathParticles;

        internal Transform MeshTransform;

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

        public virtual void Initialize(int health, CharacterController target)
        {
        }
    }
}