using System;
using Features.Actions;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class LootItem
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(LootItem), OnActivation)
                {
                    ActivationWithResultAction = OnActivationWithResult
                };
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload) 
            => OnActivationWithResult(payload);
        
        private static ActionActivationResult OnActivationWithResult(ActionActivationPayload payload)
        {
            if (payload is not LootItemActionPayload lootItemActionPayload) return ActionActivationResult.NoResultActivation;

            var targetInventory = payload.Target.transform.root.GetComponentInChildren<InventoryController>();

            if (!targetInventory) return ActionActivationResult.NoResultActivation;

            var request = ChangeRequestFactory.Add(lootItemActionPayload.Item.StorageData);

            var result = targetInventory.HandleRequest(request);

            return new ActionActivationResult(result.IsSuccess);
        }
    }

    public class LootItemActionPayload : ActionActivationPayload
    {
        public readonly ItemInstance Item;

        public LootItemActionPayload(ActionActivationPayload original, ItemInstance item) : base(original.Action,
            original.Source, original.Target)
        {
            Item = item;
        }
    }
}