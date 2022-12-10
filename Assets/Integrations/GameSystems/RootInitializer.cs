using Features.Targeting;
using UnityEngine;

namespace Integrations.GameSystems
{
    public static class RootInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            var root = new GameObject("ROOT_SYSTEMS");

            var particleSystemCore = new GameObject("particles_player_system");

            particleSystemCore.transform.SetParent(root.transform);

            particleSystemCore.AddComponent<ParticlePlayer>();

            var targetingSystem = new GameObject("target_provider_system");

            targetingSystem.transform.SetParent(root.transform);

            targetingSystem.AddComponent<TargetingManager>();
        }
    }
}