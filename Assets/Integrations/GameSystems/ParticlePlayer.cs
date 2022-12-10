using System;
using Features.Targeting;
using UnityEngine;

namespace Integrations.GameSystems
{
    public class ParticlePlayer : MonoBehaviour
    {
        public void PlayParticles(ParticleSystem particleSystem, Vector3 location)
        {
            Destroy(Instantiate(particleSystem, location, Quaternion.identity).gameObject, 2f);
        }
    }
}