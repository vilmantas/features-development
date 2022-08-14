using System.Linq;
using Features.Inventory;
using Features.Stats.Base;
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

            Item_SO[] allItems = Resources.LoadAll<Item_SO>("");

            foreach (var item in allItems)
            {
                ItemMetadataRegistry.RegisterMetadata(ToMetadata(item));
            }
        }

        public static StorageData Factory(StorageData original)
        {
            var item = original.ParentCast<ItemInstance>();

            return new ItemInstance(item.Metadata).StorageData;
        }

        private static ItemMetadata ToMetadata(Item_SO prefab)
        {
            return new(
                prefab.Name,
                prefab.Sprite,
                prefab.MaxStack,
                new StatGroup(prefab.Stats.ToArray()),
                prefab.MainSlot,
                prefab.SecondarySlot,
                prefab.ModelPrefab,
                prefab.Buffs.Select(x => x.Base).ToList(),
                prefab.Action.Base);
        }

        public static ItemInstance ToInstance(ItemMetadata metadata, int count)
        {
            var instance = new ItemInstance(metadata);
            instance.StorageData.StackableData.Receive(count);
            return instance;
        }
    }
}