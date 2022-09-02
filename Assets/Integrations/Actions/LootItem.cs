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
            ActionImplementation implementation = new(nameof(LootItem), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not LootItemActionPayload lootItemActionPayload) return;

            var targetInventory = payload.Target.transform.root.GetComponentInChildren<InventoryController>();

            if (!targetInventory) return;

            var request = ChangeRequestFactory.Add(lootItemActionPayload.Item.StorageData);

            targetInventory.HandleRequest(request);
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