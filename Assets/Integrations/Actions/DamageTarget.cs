using System;
using System.Collections.Generic;
using System.ComponentModel;
using Features.Actions;
using Features.Health;
using Integrations.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class DamageTarget
    {
        public static DamageTargetActionPayload MakePayload(GameObject source, GameObject target, int amount)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(DamageTarget)), source, target);

            return new DamageTargetActionPayload(basePayload, amount);
        }
        
        public static DamageTargetActionPayload MakePayloadForItem(GameObject source, GameObject target, ItemInstance item)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(DamageTarget)), source, target,
                new Dictionary<string, object>() {{"item", item}});

            return PayloadForItem(basePayload, item);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(DamageTarget), OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var damageActionPayload = payload as DamageTargetActionPayload;
            
            var actionController = payload.Target.GetComponentInChildren<ActionsController>();

            if (!actionController) return; // For collisions with ground Hit Box the controller wont be present 
            
            var takeDmg = TakeDamage.MakePayload(payload.Source, payload.Target,
                damageActionPayload.DamageAmount);

            actionController.DoPassiveAction(takeDmg);
        }
        
        private static DamageTargetActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is DamageTargetActionPayload damagePayload) return damagePayload;

            var rawItem = originalPayload.Data?["item"];
            
            if (rawItem is ItemInstance item)
                return PayloadForItem(originalPayload, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to {nameof(DamageTarget)} action.");
        }
        
        private static DamageTargetActionPayload PayloadForItem(ActionActivationPayload originalPayload, ItemInstance item)
        {
            var damageAmount = item.Metadata.UsageStats["Damage"].Value;
            
            return new DamageTargetActionPayload(originalPayload, damageAmount);
        }
    }

    public class DamageTargetActionPayload : ActionActivationPayload
    {
        public readonly int DamageAmount;

        public DamageTargetActionPayload(ActionActivationPayload original, int damageAmount) : base(original.Action,
            original.Source, original.Target, original.Data)
        {
            DamageAmount = damageAmount;
        }
    }
}