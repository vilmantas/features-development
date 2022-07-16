using System;
using Features.Equipment;
using Features.Inventory.Utilities;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Items
{
    public class ItemMetadata : IInventoryItemMetadata, IEquipmentItemMetadata
    {
        public ItemMetadata(string name, Sprite sprite, int maxStack, StatGroup stats, string mainSlot,
            string secondarySlot, GameObject modelPrefab)
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

        public ItemMetadata(string name, Sprite sprite, int maxStack, GameObject modelPrefab)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack < 1 ? 1 : maxStack;
            Stats = new StatGroup(Array.Empty<Stat>());
            MainSlot = string.Empty;
            SecondarySlot = string.Empty;
            IsStackable = MaxStack > 1;
            ModelPrefab = modelPrefab;
        }

        public StatGroup Stats { get; }

        public ItemInstance ToInstance => new ItemInstance(this);
        public GameObject ModelPrefab { get; }
        public string MainSlot { get; }
        public string SecondarySlot { get; }
        public bool IsStackable { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
        public int MaxStack { get; }
    }
}