using System.Collections.Generic;
using System.Linq;
using Features.Buffs;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Items
{
    [CreateAssetMenu(menuName = "Items/Item", fileName = "RENAME_ME")]
    public class Item_SO : ScriptableObject
    {
        public string Name;
        public string MainSlot;
        public string SecondarySlot;
        public int Count;
        public int MaxStack;
        public Sprite Sprite;
        public GameObject ModelPrefab;
        public List<Stat> Stats;
        public Buff_SO[] Buffs;

        public ItemMetadata ToMetadata => new(Name, Sprite, MaxStack,
            new StatGroup(Stats.ToArray()), MainSlot, SecondarySlot, ModelPrefab, Buffs.Select(x => x.Base).ToList());

        public ItemInstance ToInstance()
        {
            var instance = new ItemInstance(ToMetadata);
            instance.StorageData.StackableData.Receive(Count);
            return instance;
        }
    }
}