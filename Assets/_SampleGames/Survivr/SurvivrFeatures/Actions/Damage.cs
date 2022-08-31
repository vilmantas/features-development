using Features.Actions;
using Features.Health;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Actions
{
    public static class Damage
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new("Damage", OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not DamageActionPayload damageActionPayload) return;

            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Damage(damageActionPayload.DamageAmount);
        }

        private static DamageActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is DamageActionPayload damageActionPayload) return damageActionPayload;

            return new(originalPayload, 5);
        }
    }

    public class DamageActionPayload : ActionActivationPayload
    {
        public readonly int DamageAmount;

        public DamageActionPayload(ActionActivationPayload original, int damageAmount) : base(original.Action,
            original.Source, original.Target)
        {
            DamageAmount = damageAmount;
        }
    }
}