using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.Equipment
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

        public bool CanEquipItem(IEquipmentItemInstance itemInstance)
        {
            return SlotPresent(itemInstance.Metadata.MainSlot) || SlotPresent(itemInstance.Metadata.SecondarySlot);
        }

        public EquipmentContainerItem EquipOrReplace(
            string slot,
            IEquipmentItemInstance itemInstance,
            out IEquipmentItemInstance previousItemInstance,
            out bool combinationSuccess
            )
        {
            combinationSuccess = false;
            
            previousItemInstance = null;

            if (!SlotPresent(slot)) return null;

            var equippedItem = EmptySlotPresent(slot) ? EmptySlot(slot) : FirstSlot(slot);

            combinationSuccess = TryCombine(itemInstance, equippedItem);

            if (combinationSuccess) return equippedItem;
            
            return EquipToSlot(itemInstance, equippedItem, out previousItemInstance);
        }

        public EquipmentContainerItem EquipOrReplace(
            Guid slotId,
            IEquipmentItemInstance itemInstance,
            out IEquipmentItemInstance previousItemInstance,
            out bool combinationSuccess
            )
        {
            combinationSuccess = false;
            
            previousItemInstance = null;

            var equipmentSlot = SlotById(slotId);

            if (equipmentSlot == null) return null;

            combinationSuccess = TryCombine(itemInstance, equipmentSlot);

            if (combinationSuccess) return equipmentSlot;

            return EquipToSlot(itemInstance, equipmentSlot, out previousItemInstance);
        }

        private static EquipmentContainerItem EquipToSlot(IEquipmentItemInstance itemInstance, EquipmentContainerItem equipmentSlot,
            out IEquipmentItemInstance previousItemInstance)
        {
            previousItemInstance = equipmentSlot.Main;

            equipmentSlot.Main = itemInstance;

            return equipmentSlot;
        }

        private static bool TryCombine(IEquipmentItemInstance itemInstance,
            EquipmentContainerItem slot)
        {
            return !slot.IsEmpty && slot.Main.Combine(itemInstance);
        }
    }
}