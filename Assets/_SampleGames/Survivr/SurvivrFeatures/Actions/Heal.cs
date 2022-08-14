using Features.Actions;
using Features.Health;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Actions
{
    public class Heal
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new("Heal", OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Heal(10);
        }
    }
}