using System;
using Features.Actions;
using Features.Health;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Heal
    {
        public static HealActionPayload MakePayload(GameObject source, GameObject target, int amount)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Heal)), source, target);

            return new HealActionPayload(basePayload, amount);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Heal), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not HealActionPayload healPayload) return;
            
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Heal(healPayload.HealAmount);
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