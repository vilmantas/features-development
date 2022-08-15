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
            ActionImplementation implementation = new("Heal", OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not HealActionPayload healPayload)
            {
                Debug.LogWarning("Invalid payload for heal action.");
                return;
            }

            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Heal(healPayload.HealAmount);
        }

        private static HealActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            return new(originalPayload, 10);
        }

        public class HealActionPayload : ActionActivationPayload
        {
            public readonly int HealAmount;

            public HealActionPayload(ActionActivationPayload original, int healAmount) : base(original.Action,
                original.Source, original.Target)
            {
                HealAmount = healAmount;
            }
        }
    }
}