using System;

namespace Features.Equipment
{
    public class EquipRequest
    {
        public IEquipmentItemInstance ItemInstance;
        public Guid SlotId;
        public string SlotType;

        public bool IsForSpecificSlot => !string.IsNullOrEmpty(SlotType);
        public IEquipmentItemInstance<T> SourceItem<T>() where T : class => ItemInstance as IEquipmentItemInstance<T>;
    }
}