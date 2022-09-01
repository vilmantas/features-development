using System;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Equip
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Equip), OnActivation, OnPayloadMake);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            var equipActionPayload = payload as EquipActionPayload;

            var equipmentController = payload.Target.GetComponentInChildren<EquipmentController>();
            
            equipmentController.HandleEquipRequest(equipActionPayload.EquipRequest);
        }

        private static EquipActionPayload OnPayloadMake(ActionActivationPayload original)
        {
            var item = original.Source as ItemInstance;

            var request = new EquipRequest() { ItemInstance = item };

            return new EquipActionPayload(original, request, item);
        }
    }

    public class EquipActionPayload : ActionActivationPayload
    {
        public EquipRequest EquipRequest { get; }
        public ItemInstance ItemInstance { get; }

        public EquipActionPayload(ActionActivationPayload original, EquipRequest request, ItemInstance itemInstance) : base(original.Action,
            original.Source, original.Target)
        {
            EquipRequest = request;
            ItemInstance = itemInstance;
        }
    }
}