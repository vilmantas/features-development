using System;
using Features.Actions;
using Features.Health;
using Integrations.Items;
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
            ActionImplementation implementation = new(nameof(Heal), OnActivation, PayloadMake);
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

            var rawItem = originalPayload.Data?["item"];
            
            if (rawItem is ItemInstance item)
                return PayloadForItem(originalPayload, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to heal action {originalPayload.GetType().Name}");
        }

        private static HealActionPayload PayloadForItem(ActionActivationPayload originalPayload, ItemInstance item)
        {
            var healAmount = item.Metadata.UsageStats["Healing"].Value;
            
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