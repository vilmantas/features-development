using System.Collections.Generic;
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

        public ItemInstance MakeInstance()
        {
            var metadata = ItemMetadataRegistry.Registry[Name];

            var instance = ItemFactory.ToInstance(metadata, Count);

            return instance;
        }
    }
}