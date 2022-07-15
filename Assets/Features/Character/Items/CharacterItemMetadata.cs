using System;
using Features.Equipment;
using Features.Inventory.Utilities;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Character.Items
{
    public class CharacterItemMetadata : IInventoryItemMetadata, IEquipmentItemMetadata
    {
        public string Name { get; }
        public GameObject ModelPrefab { get; }
        public string MainSlot { get; }
        public string SecondarySlot { get; }
        public Sprite Sprite { get; }
        public bool IsStackable { get; }
        public int MaxStack { get; }
        public StatGroup Stats { get; }

        public CharacterItemMetadata(string name, Sprite sprite, int maxStack, StatGroup stats, string mainSlot, string secondarySlot, GameObject modelPrefab)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack < 1 ? 1 : maxStack;
            Stats = stats;
            MainSlot = mainSlot;
            SecondarySlot = secondarySlot;
            IsStackable = MaxStack > 1;
            ModelPrefab = modelPrefab;
        }

        public CharacterItemInstance ToInstance => new CharacterItemInstance(this);
    }
}