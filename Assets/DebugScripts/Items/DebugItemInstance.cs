using System;
using Features.Inventory.UI.Utilities;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class DebugItemInstance : IInventoryItemInstance, IEquatable<object>
    {
        public readonly DebugEquipmentData DebugEquipmentData;
        public readonly DebugItemMetadata Metadata;

        public readonly StorageData StorageData;

        public DebugItemInstance(DebugItemMetadata metadata)
        {
            Metadata = metadata;

            DebugEquipmentData = new DebugEquipmentData(this, "", "");

            StorageData = new StorageData(this, metadata.MaxStack);
        }

        public override bool Equals(object other)
        {
            if (other is not DebugItemInstance b) return false;

            return Metadata.Name.Equals(b.Metadata.Name);
        }

        IInventoryItemMetadata IInventoryItemInstance.Metadata => Metadata;

        public static bool operator !=(DebugItemInstance a, DebugItemInstance b)
        {
            return a?.Metadata.Name != b?.Metadata.Name;
        }

        public static bool operator ==(DebugItemInstance a, DebugItemInstance b)
        {
            return a?.Metadata.Name == b?.Metadata.Name;
        }

        public override int GetHashCode()
        {
            return Metadata.Name.GetHashCode();
        }
    }
}