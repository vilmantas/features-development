using System;
using System.Collections.Generic;
using Features.Buffs;
using Features.Equipment;
using Features.Inventory.UI.Utilities;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Items
{
    public class ItemMetadata : IInventoryItemMetadata, IEquipmentItemMetadata
    {
        public readonly Guid Id = Guid.NewGuid();

        public ItemMetadata(string name, Sprite sprite, int maxStack, StatGroup stats, string mainSlot,
            string secondarySlot, GameObject modelPrefab, List<BuffBase> buffs)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack < 1 ? 1 : maxStack;
            Stats = stats;
            MainSlot = mainSlot;
            SecondarySlot = secondarySlot;
            IsStackable = MaxStack > 1;
            ModelPrefab = modelPrefab;
            Buffs = buffs;
        }

        public ItemMetadata(string name, Sprite sprite, int maxStack, GameObject modelPrefab, List<BuffBase> buffs)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack < 1 ? 1 : maxStack;
            Stats = new StatGroup(Array.Empty<Stat>());
            MainSlot = string.Empty;
            SecondarySlot = string.Empty;
            IsStackable = MaxStack > 1;
            ModelPrefab = modelPrefab;
            Buffs = buffs;
        }

        public StatGroup Stats { get; }

        public ItemInstance ToInstance => new(this);

        public List<BuffBase> Buffs { get; }
        public GameObject ModelPrefab { get; }
        public string MainSlot { get; }
        public string SecondarySlot { get; }
        public bool IsStackable { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
        public int MaxStack { get; }
    }
}