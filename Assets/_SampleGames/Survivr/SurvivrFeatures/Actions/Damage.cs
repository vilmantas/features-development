using System.ComponentModel;
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
            ActionImplementation implementation = new(nameof(Damage), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var damageActionPayload = payload as DamageActionPayload;
            
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Damage(damageActionPayload.DamageAmount);
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