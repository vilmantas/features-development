using System;
using System.ComponentModel;
using Features.Actions;
using Features.Health;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Damage
    {
        public static DamageActionPayload MakePayload(object source, GameObject target, int amount)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Damage)), source, target);

            return new DamageActionPayload(basePayload, amount);
        }
        
        public static DamageActionPayload MakePayloadForItem(object source, GameObject target, ItemInstance item)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Damage)), source, target);

            return PayloadForItem(basePayload, item);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Damage), OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var damageActionPayload = payload as DamageActionPayload;
            
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Damage(damageActionPayload.DamageAmount);
        }
        
        private static DamageActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is DamageActionPayload damagePayload) return damagePayload;

            if (originalPayload.Source is ItemInstance item)
                return PayloadForItem(originalPayload, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to damage action {originalPayload.GetType().Name}");
        }
        
        private static DamageActionPayload PayloadForItem(ActionActivationPayload originalPayload, ItemInstance item)
        {
            var damageAmount = item.Metadata.UsageStats["Damage"].Value;
            
            return new DamageActionPayload(originalPayload, damageAmount);
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