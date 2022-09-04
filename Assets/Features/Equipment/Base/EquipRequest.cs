using System;

namespace Features.Equipment
{
    public class EquipRequest
    {
        public IEquipmentItemInstance Item;
        public bool PreventDefault;
        public string Slot;
        public Guid SlotId;

        public bool IsForSpecificSlot => !string.IsNullOrEmpty(Slot);
    }
}