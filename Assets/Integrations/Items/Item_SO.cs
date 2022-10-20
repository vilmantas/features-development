using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Buffs;
using Features.Stats.Base;
using UnityEngine;

namespace Features.Items
{
    [CreateAssetMenu(menuName = "Items/New Item", fileName = "New Item")]
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
        public Action_SO Action;
        public List<Action_SO> InventoryActions;
        public List<Action_SO> EquipmentActions;
        public string AttackAnimation = string.Empty;

        public ItemInstance MakeInstanceWithCount()
        {
            var metadata = ItemMetadataRegistry.Registry[Name];

            var instance = ItemFactory.CreateInstanceFrom(metadata, Count);

            return instance;
        }
    }
}