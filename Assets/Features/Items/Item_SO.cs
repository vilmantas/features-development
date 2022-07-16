using System.Collections.Generic;
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
        public Sprite Sprite;
        public GameObject ModelPrefab;
        public List<Stat> Stats;

        public ItemMetadata ToMetadata => new(Name, Sprite, 1,
            new StatGroup(Stats.ToArray()), MainSlot, SecondarySlot, ModelPrefab);
    }
}