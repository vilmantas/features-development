using System;
using Features.Actions;
using Features.Inventory;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class LootItem
    {
        public static LootItemActionPayload MakePayload(GameObject source, GameObject target, ItemInstance item)
        {
            var basePaylaod = new ActionActivationPayload(new ActionBase(nameof(LootItem)), source, target);

            return new LootItemActionPayload(basePaylaod, item);
        }
        
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

            return new LootItemActionResult(result.IsSuccess, result as AddRequestResult);
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

    public class LootItemActionResult : ActionActivationResult
    {
        public AddRequestResult LootResult;
        
        public LootItemActionResult(bool? isSuccessful, AddRequestResult lootResult) : base(isSuccessful)
        {
            LootResult = lootResult;
        }
    }
}