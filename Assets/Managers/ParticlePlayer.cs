using UnityEngine;

namespace Managers
{
    public class ParticlePlayer : SingletonManager<ParticlePlayer>
    {
        public void PlayParticles(ParticleSystem particles, Vector3 location)
        {
            Destroy(Instantiate(particles, location, Quaternion.identity).gameObject, 2f);
        }

        public GameObject CreateInstanceOf(GameObject obj)
        {
            var z = Instantiate(obj);

            Destroy(z, 1f);
            return z;
        }
    }
}