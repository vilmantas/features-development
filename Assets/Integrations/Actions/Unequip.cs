using System;
using Features.Actions;
using Features.Equipment;
using UnityEngine;

namespace Integrations.Actions
{
    public static class Unequip
    {
        public static UnequipActionPayload MakePayload(GameObject source, GameObject target,
            EquipmentContainerItem request)
        {
            var basePayload = new ActionActivationPayload(new ActionBase(nameof(Unequip)), source, target);

            return new UnequipActionPayload(basePayload, request);
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            ActionImplementation implementation = new(nameof(Unequip), OnActivation);
            ActionImplementationRegistry.Implementations.TryAdd(implementation.Name, implementation);
        }

        private static void OnActivation(ActionActivationPayload payload)
        {
            if (payload is not UnequipActionPayload unequipActionPayload)
            {
                throw new ArgumentException("Invalid type of payload passed to unequip action");
            }

            var equipmentController = payload.Target.GetComponentInChildren<EquipmentController>();

            equipmentController.UnequipItem(new() {ContainerItem = unequipActionPayload.ContainerSlot});
        }
    }

    public class UnequipActionPayload : ActionActivationPayload
    {
        public UnequipActionPayload(ActionActivationPayload original, EquipmentContainerItem containerSlot) : base(
            original.Action,
            original.Source, original.Target)
        {
            ContainerSlot = containerSlot;
        }

        public EquipmentContainerItem ContainerSlot { get; }
    }
}