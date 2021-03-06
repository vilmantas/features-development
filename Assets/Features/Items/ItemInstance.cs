using System;
using Features.Equipment;
using Features.Inventory.Utilities;
using Utilities.ItemsContainer;

namespace Features.Items
{
    public class ItemInstance : IInventoryItemInstance, IEquipmentItemInstance, IEquatable<object>
    {
        public readonly Guid Id = Guid.NewGuid();

        public ItemInstance(ItemMetadata metadata)
        {
            Metadata = metadata;
            StorageData = new StorageData<ItemInstance>(this, Metadata.MaxStack);
        }

        public ItemMetadata Metadata { get; }
        public StorageData<ItemInstance> StorageData { get; }
        public string GetAmmoText => StorageData.Max > 1 ? StorageData.Current.ToString() : string.Empty;
        public int CurrentAmount => StorageData.StackableData.Current;
        IEquipmentItemMetadata IEquipmentItemInstance.Metadata => Metadata;

        public bool Combine(IEquipmentItemInstance other)
        {
            if (other is not ItemInstance otherItem) return false;

            if (!otherItem.Metadata.Name.Equals(Metadata.Name)) return false;

            if (!Metadata.IsStackable) return false;

            var amountToAdd = otherItem.CurrentAmount;

            StorageData.StackableData.Receive(amountToAdd,
                out int leftovers);

            if (amountToAdd == leftovers)
            {
                return false;
            }

            otherItem.StorageData.StackableData.Reduce(amountToAdd - leftovers, out _);

            return true;
        }

        IInventoryItemMetadata IInventoryItemInstance.Metadata => Metadata;

        public override bool Equals(object obj)
        {
            if (obj is not ItemInstance instance) return false;

            return instance.Metadata.Name.Equals(Metadata.Name);
        }
    }
}