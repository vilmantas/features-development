using System;
using Features.Actions;
using Features.Equipment;
using Features.Items;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Equip
    {
        public static EquipActionPayload MakePayload(GameObject source, GameObject target,
            EquipRequest request)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Heal)), source, target);

            return new EquipActionPayload(basePayload, request);
        }
        
        public static EquipActionPayload MakePayload(ActionActivationPayload original, ItemInstance item)
        {
            return new EquipActionPayload(original, new EquipRequest() {Item = item});
        }
        
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

            if (!equipmentController) return;
            
            equipmentController.HandleEquipRequest(equipActionPayload.EquipRequest);
        }

        private static EquipActionPayload OnPayloadMake(ActionActivationPayload original)
        {
            if (original is EquipActionPayload equipActionPayload) return equipActionPayload;

            var rawItem = original.Data?["item"];
            
            if (rawItem is ItemInstance item)
                return MakePayload(original, item);
            
            throw new InvalidOperationException(
                $"Invalid payload passed to {nameof(Equip)} action.");
        }
    }

    public class EquipActionPayload : ActionActivationPayload
    {
        public EquipActionPayload(ActionActivationPayload original, EquipRequest request) :
            base(original.Action,
                original.Source, original.Target)
        {
            EquipRequest = request;
        }

        public EquipRequest EquipRequest { get; }
    }
}