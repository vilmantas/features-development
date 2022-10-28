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
                var metadata = ToMetadata(item);
                
                ItemMetadataRegistry.Register(metadata);
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
                new StatGroup(prefab.EquipStats.ToArray()),
                prefab.MainSlot,
                prefab.SecondarySlot,
                prefab.ModelPrefab,
                prefab.Buffs?.Select(x => x.Base.WithInterval(x.Interval, x.ExecuteTickImmediately)).ToList(),
                prefab.Action == null ? null : prefab.Action.Base,
                prefab.InventoryActions.Select(x => x.Base).ToList(),
                prefab.EquipmentActions.Select(x => x.Base).ToList(),
                prefab.AttackAnimation,
                new StatGroup(prefab.UsageStats.ToArray()),
                prefab.RequiredAmmoType,
                prefab.ProvidedAmmo);
        }

        public static ItemInstance CreateInstanceFrom(ItemMetadata metadata, int count = 0)
        {
            var instance = new ItemInstance(metadata);
            instance.StorageData.StackableData.Receive(count);
            return instance;
        }
    }
}