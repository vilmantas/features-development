using System;

namespace Features.Equipment
{
    public class EquipmentContainerItem
    {
        public readonly Guid Id = Guid.NewGuid();

        public IEquipmentItem Main;

        public string Slot;

        public bool IsEmpty => Main == null;
    }
}