using System;
using System.Linq;
using Features.Actions;
using Features.Buffs;
using Features.Health;
using Features.Inventory;
using Integrations.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Consume
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Consume), OnActivation, OnPayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not ConsumeActionPayload consumeActionPayload) return;

            var itemBuffs = consumeActionPayload.Item.Metadata.Buffs;
            
            if (itemBuffs != null && itemBuffs.Any())
            {
                var m_BuffController = consumeActionPayload.Target
                    .GetComponentInChildren<BuffController>();
                
                foreach (var metadataBuff in itemBuffs)
                {
                    m_BuffController.AttemptAdd(new BuffAddOptions(metadataBuff, consumeActionPayload.Target, 1));
                }
            }

            var healing = consumeActionPayload.Item.Metadata.UsageStats["Healing"].Value;

            if (healing != 0)
            {
                var health = consumeActionPayload.Target.GetComponentInChildren<HealthController>();

                if (health)
                {
                    health.Heal(healing);
                }
            }

            consumeActionPayload.Target.GetComponentInChildren<InventoryController>()
                .HandleRequest(
                    ChangeRequestFactory.RemoveExact(consumeActionPayload.Item.StorageData));
        }

        private static ConsumeActionPayload OnPayloadMake(ActionActivationPayload original)
        {
            if (original is ConsumeActionPayload consumeActionPayload) return consumeActionPayload;

            var rawItem = original.Data?["item"];
            
            if (rawItem is ItemInstance item)
                return new(original, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to {nameof(Consume)} action.");
        }
    }

    public class ConsumeActionPayload : ActionActivationPayload
    {
        public readonly ItemInstance Item;

        public ConsumeActionPayload(ActionActivationPayload original, ItemInstance item) : base(original.Action,
            original.Source, original.Target)
        {
            Item = item;
        }
    }
}