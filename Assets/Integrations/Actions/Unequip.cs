using System;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Inventory;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Unequip
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Unequip), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var equipActionPayload = payload as UnequipActionPayload;

            var equipmentController = payload.Target.GetComponentInChildren<EquipmentController>();
            
            var instance = equipActionPayload.ContainerSlot.Main as ItemInstance;
            
            if (instance == null) return;

            var inventoryController = payload.Target.GetComponentInChildren<InventoryController>();
            
            int maxAmountToAdd;

            if (!inventoryController)
            {
                UnequipItem(equipmentController, equipActionPayload);

                return;
            }
            
            if (!inventoryController.CanReceive(instance.StorageData, out maxAmountToAdd)) return;
            
            if (maxAmountToAdd >= instance.CurrentAmount)
            {
                UnequipItem(equipmentController, equipActionPayload);
            }
            else
            {
                ReduceWithoutUnequipping(inventoryController, instance, equipmentController, equipActionPayload);
            }
        }

        private static void ReduceWithoutUnequipping(InventoryController inventoryController,
            ItemInstance instance, EquipmentController equipmentController,
            UnequipActionPayload equipActionPayload)
        {
            var result = inventoryController.HandleRequest(
                ChangeRequestFactory.Add(instance.StorageData)) as AddRequestResult;


            instance.StorageData.StackableData.Reduce(result.AmountAdded);

            equipmentController.NotifyItemChanged(equipActionPayload.ContainerSlot);
        }

        private static void UnequipItem(EquipmentController equipmentController,
            UnequipActionPayload equipActionPayload)
        {
            equipmentController.HandleEquipRequest(new EquipRequest()
                {ItemInstance = null, SlotType = equipActionPayload.ContainerSlot.Slot});
        }
    }

    public class UnequipActionPayload : ActionActivationPayload
    {
        public EquipmentContainerItem ContainerSlot { get; }

        public UnequipActionPayload(ActionActivationPayload original, EquipmentContainerItem containerSlot) : base(original.Action,
            original.Source, original.Target)
        {
            ContainerSlot = containerSlot;
        }
    }
}