using System;
using Features.Actions;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace _SampleGames.Survivr.SurvivrFeatures.Actions
{
    public static class PickupItem
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new("PickupItem", OnActivation, PayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not PickupItemActionPayload pickupItemActionPayload) return;

            var targetInventory = payload.Target.transform.root.GetComponentInChildren<InventoryController>();

            if (!targetInventory) return;

            var request = ChangeRequestFactory.Add(pickupItemActionPayload.Item.StorageData);

            targetInventory.HandleRequest(request);
        }

        private static PickupItemActionPayload PayloadMake(ActionActivationPayload originalPayload)
        {
            if (originalPayload is PickupItemActionPayload pickupItemActionPayload) return pickupItemActionPayload;

            throw new InvalidOperationException($"Pickup Item action for {originalPayload.Target.transform.root.name} received an empty payload");
        }
    }

    public class PickupItemActionPayload : ActionActivationPayload
    {
        public readonly ItemInstance Item;

        public PickupItemActionPayload(ActionActivationPayload original, ItemInstance item) : base(original.Action,
            original.Source, original.Target)
        {
            Item = item;
        }
    }
}