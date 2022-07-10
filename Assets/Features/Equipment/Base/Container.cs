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

            return !Manager.CanEquipItem(request.Item) ? Failed(request) : EquipByItem(request);
        }

        private EquipResult EquipByItem(EquipRequest request)
        {
            var item = request.Item;
            var slot = item.mainSlot;

            if (!Manager.EmptySlotPresent(item.mainSlot) && Manager.EmptySlotPresent(item.secondarySlot))
            {
                slot = item.secondarySlot;
            }

            var result = Manager.EquipOrReplace(slot, request.Item, out var prevItem);

            return EquipResult(request, result, prevItem);
        }

        private EquipResult EquipBySlotType(EquipRequest request)
        {
            var item = Manager.EquipOrReplace(request.SlotType, request.Item, out var prevItem);

            return EquipResult(request, item, prevItem);
        }

        private EquipResult EquipBySlotId(EquipRequest request)
        {
            var item = Manager.EquipOrReplace(request.SlotId, request.Item, out var prevItem);

            return EquipResult(request, item, prevItem);
        }

        private EquipResult EquipResult(EquipRequest request, EquipmentContainerItem equipmentContainerItem,
            IEquipmentItem prevItem)
        {
            return new EquipResult(request, equipmentContainerItem, prevItem, true);
        }

        private static EquipResult Failed(EquipRequest request)
        {
            return new EquipResult(request);
        }
    }
}