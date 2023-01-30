using Managers;
using UnityEngine;

namespace Features.ParticlePlayer
{
    public class ParticlePlayerController : SingletonManager<ParticlePlayerController>
    {
        public void PlayParticles(ParticleSystem particles, Vector3 location)
        {
            Destroy(Instantiate(particles, location, Quaternion.identity).gameObject, 2f);
        }
    }
}