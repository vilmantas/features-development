using System;
using Features.Actions;
using Features.Health;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
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
            var healPayload = payload as HealActionPayload;
            
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Heal(healPayload.HealAmount);
        }

        private static HealActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is HealActionPayload healActionPayload) return healActionPayload;

            if (originalPayload.Source is ItemInstance item)
                return PayloadForItem(originalPayload, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to heal action {originalPayload.GetType().Name}");
        }

        private static HealActionPayload PayloadForItem(ActionActivationPayload originalPayload, ItemInstance item)
        {
            var healAmount = item.Metadata.Stats["Healing"].Value;
            
            return new HealActionPayload(originalPayload, healAmount);
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