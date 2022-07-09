using System;

namespace Equipment
{
    public class EquipmentContainerItem
    {
        public readonly Guid Id = Guid.NewGuid();

        public IEquipmentItem Main;

        public string Slot;
    }
}