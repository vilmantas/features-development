using System;
using System.Collections.Generic;
using System.Linq;

namespace Equipment
{
    public class SlotManager
    {
        private readonly List<EquipmentContainerItem> m_EquippedItems = new();

        public readonly string[] Slots;

        public SlotManager(string[] slots)
        {
            slots = slots.Where(x => x != String.Empty).ToArray();

            Slots = slots;

            foreach (var slot in slots)
            {
                m_EquippedItems.Add(new EquipmentContainerItem() {Main = null, Slot = slot});
            }
        }

        public IReadOnlyList<EquipmentContainerItem> Items => m_EquippedItems;

        public bool SlotPresent(string slot) => Slots.Any(x => x == slot);

        public EquipmentContainerItem SlotById(Guid slotId) => Items.FirstOrDefault(x => x.Id == slotId);

        public bool EmptySlotPresent(string slot) => m_EquippedItems.Any(x => x.Slot == slot && x.Main == null);

        private EquipmentContainerItem EmptySlot(string slot) =>
            m_EquippedItems.First(x => x.Slot == slot && x.Main == null);

        private EquipmentContainerItem FirstSlot(string slot) => m_EquippedItems.First(x => x.Slot == slot);

        public bool CanEquipItem(IEquipmentItem item)
        {
            return SlotPresent(item.mainSlot) || SlotPresent(item.secondarySlot);
        }

        public EquipmentContainerItem EquipOrReplace(string slot, IEquipmentItem item, out IEquipmentItem previousItem)
        {
            previousItem = null;

            if (!SlotPresent(slot)) return null;

            var equippedItem = EmptySlotPresent(slot) ? EmptySlot(slot) : FirstSlot(slot);

            return EquipmentSlot(item, equippedItem, out previousItem);
        }

        public EquipmentContainerItem EquipOrReplace(Guid slotId, IEquipmentItem item, out IEquipmentItem previousItem)
        {
            previousItem = null;

            var equipmentSlot = SlotById(slotId);

            return equipmentSlot != null ? EquipmentSlot(item, equipmentSlot, out previousItem) : null;
        }

        private static EquipmentContainerItem EquipmentSlot(IEquipmentItem item, EquipmentContainerItem equipmentSlot,
            out IEquipmentItem previousItem)
        {
            previousItem = null;

            if (equipmentSlot.Main != null && equipmentSlot.Main.Combine(item)) return equipmentSlot;

            previousItem = equipmentSlot.Main;

            equipmentSlot.Main = item;

            return equipmentSlot;
        }
    }
}