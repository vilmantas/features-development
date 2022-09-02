using System;
using System.Collections.Generic;

namespace Features.Equipment
{
    public class Container
    {
        private SlotManager Manager;

        public Container(string[] slots)
        {
            Manager = new SlotManager(slots);
        }

        public IReadOnlyList<EquipmentContainerItem> ContainerSlots => Manager.Items;

        public EquipResult Equip(EquipRequest request)
        {
            if (request.SlotId != Guid.Empty)
            {
                return EquipBySlotId(request);
            }

            if (request.IsForSpecificSlot)
            {
                return EquipBySlotType(request);
            }

            return !Manager.CanEquipItem(request.ItemInstance) ? Failed(request) : EquipByItem(request);
        }

        private EquipResult EquipByItem(EquipRequest request)
        {
            var item = request.ItemInstance;
            var slot = item.Metadata.MainSlot;

            if (!Manager.EmptySlotPresent(slot) && Manager.EmptySlotPresent(item.Metadata.SecondarySlot))
            {
                slot = item.Metadata.SecondarySlot;
            }

            var result = Manager.EquipOrReplace(slot, request.ItemInstance, out var prevItem, out var combined);

            return EquipResult(request, result, prevItem, combined);
        }

        private EquipResult EquipBySlotType(EquipRequest request)
        {
            var item = Manager.EquipOrReplace(request.SlotType, request.ItemInstance, out var prevItem, out var combined);

            return EquipResult(request, item, prevItem, combined);
        }

        private EquipResult EquipBySlotId(EquipRequest request)
        {
            var item = Manager.EquipOrReplace(request.SlotId, request.ItemInstance, out var prevItem, out var combined);

            return EquipResult(request, item, prevItem, combined);
        }

        private EquipResult EquipResult(EquipRequest request, EquipmentContainerItem equipmentContainerItem,
            IEquipmentItemInstance prevItemInstance, bool itemsCombined)
        {
            return new EquipResult(request, equipmentContainerItem, prevItemInstance, itemsCombined, true);
        }

        private static EquipResult Failed(EquipRequest request)
        {
            return new EquipResult(request);
        }
    }
}