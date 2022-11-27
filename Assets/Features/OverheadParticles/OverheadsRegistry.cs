using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Features.OverheadParticles
{
    public class OverheadsRegistry
    {
        public static IReadOnlyDictionary<string, ParticleSystem> Implementations =>
            m_Implementations;

        private static readonly ConcurrentDictionary<string, ParticleSystem> m_Implementations =
            new();

        public static void Register(string name, ParticleSystem particles)
        {
            if (Implementations.ContainsKey(name)) return;

            m_Implementations.TryAdd(name, particles);
        }
    }
}