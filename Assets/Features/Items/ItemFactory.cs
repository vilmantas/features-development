using Features.Inventory;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Items
{
    public static class ItemFactory
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RegisterFactory()
        {
            InventoryItemFactories.Register(typeof(ItemInstance), Factory);
        }

        public static StorageData Factory(StorageData original)
        {
            var item = original.ParentCast<ItemInstance>();

            return new ItemInstance(item.Metadata).StorageData;
        }
    }
}