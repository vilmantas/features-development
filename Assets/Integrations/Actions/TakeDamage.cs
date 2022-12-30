using System;
using System.Collections.Generic;
using System.ComponentModel;
using Features.Actions;
using Features.Health;
using Integrations.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class TakeDamage
    {
        public static TakeDamageActionPayload MakePayload(GameObject source, GameObject target, int baseAmount)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(TakeDamage)), source, target);

            return new TakeDamageActionPayload(basePayload, baseAmount);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(TakeDamage), OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var damageActionPayload = payload as TakeDamageActionPayload;
            
            var health = payload.Target.GetComponentInChildren<HealthController>();

            if (!health) return;

            health.Damage(damageActionPayload.DamageAmount);
        }
        
        private static TakeDamageActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is TakeDamageActionPayload damagePayload) return damagePayload;

            throw new InvalidOperationException(
                $"Invalid payload passed to {nameof(TakeDamage)} action.");
        }
    }

    public class TakeDamageActionPayload : ActionActivationPayload
    {
        public readonly int DamageAmount;

        public TakeDamageActionPayload(ActionActivationPayload original, int damageAmount) : base(original.Action,
            original.Source, original.Target, original.Data)
        {
            DamageAmount = damageAmount;
        }
    }
}