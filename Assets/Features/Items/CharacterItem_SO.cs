using System.Collections.Generic;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character.Items
{
    [CreateAssetMenu(menuName = "Items/Item", fileName = "RENAME_ME")]
    public class CharacterItem_SO : ScriptableObject
    {
        public string Name;
        public string MainSlot;
        public string SecondarySlot;
        public Sprite Sprite;
        public GameObject ModelPrefab;
        [Min(1)]
        public int MaxStack;
        public List<Stat> Stats;

        public CharacterItemMetadata ToMetadata => new CharacterItemMetadata(Name, Sprite, MaxStack,
            new StatGroup(Stats.ToArray()), MainSlot, SecondarySlot, ModelPrefab);
    }
}