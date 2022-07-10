using System;

namespace Features.Equipment
{
    public class EquipRequest
    {
        public IEquipmentItem Item;
        public Guid SlotId;

        public string SlotType;

        public bool IsForSpecificSlot => SlotType != string.Empty;

        public IEquipmentItem<T> SourceItem<T>() where T : class => Item as IEquipmentItem<T>;
    }
}