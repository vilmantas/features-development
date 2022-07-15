using Features.Equipment;
using Features.Inventory;
using Features.Inventory.Utilities;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Character.Items
{
    public static class ItemFactory
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void  RegisterFactory()
        {
            InventoryItemFactories.Register(typeof(CharacterItemInstance), Factory);
        }

        public static StorageData Factory(StorageData original)
        {
            var item = original.ParentCast<CharacterItemInstance>();

            return new CharacterItemInstance(item.Metadata).StorageData;
        }
    }

    public class CharacterItemInstance : IItemInstance, IEquipmentItemInstance<CharacterItemInstance>
    {
        IInventoryItemMetadata IItemInstance.Metadata => Metadata;
        IEquipmentItemMetadata IEquipmentItemInstance.Metadata => Metadata;
        public string GetAmmoText { get; }
        public int CurrentAmount => StorageData.StackableData.Current;
        public bool Combine(IEquipmentItemInstance other)
        {
            if (other is not IEquipmentItemInstance<CharacterItemInstance> otherItem) return false;

            if (!otherItem.Parent.Equals(Parent)) return false;

            if (!Metadata.IsStackable) return false;

            var amountToAdd = otherItem.CurrentAmount;

            Parent.StorageData.StackableData.Receive(amountToAdd,
                out int leftovers);

            otherItem.Parent.StorageData.StackableData.Reduce(amountToAdd - leftovers, out _);

            return true;
        }

        public CharacterItemMetadata Metadata { get; }

        public StorageData<CharacterItemInstance> StorageData { get; }

        public CharacterItemInstance(CharacterItemMetadata metadata)
        {
            Metadata = metadata;
            StorageData = new StorageData<CharacterItemInstance>(this, Metadata.MaxStack);
        }

        public CharacterItemInstance Parent { get; }
    }

}