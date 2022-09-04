using Features.Actions;
using Features.Equipment;
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

            equipmentController.UnequipItem(new() {ContainerItem = equipActionPayload.ContainerSlot});
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