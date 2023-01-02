using Features.Targeting;
using UnityEngine;

namespace Managers
{
    public static class RootInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var gameManager = new GameObject("game_manager");

            gameManager.AddComponent<GameManager>();
        }

        public static void AddGameplayNodes()
        {
            var root = new GameObject("ROOT_SYSTEMS");

            var gameplayManager = new GameObject("gameplay_manager");

            gameplayManager.transform.SetParent(root.transform);

            gameplayManager.AddComponent<GameplayManager>();

            var particleSystemCore = new GameObject("particles_player_system");

            particleSystemCore.transform.SetParent(root.transform);

            particleSystemCore.AddComponent<ParticlePlayer>();

            var targetingSystem = new GameObject("target_provider_system");

            targetingSystem.transform.SetParent(root.transform);

            targetingSystem.AddComponent<TargetingManager>();
        }
    }
}