using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Buffs;
using Features.Skills;
using Features.Stats.Base;
using Features.WeaponAnimationConfigurations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Integrations.Items
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
        [FormerlySerializedAs("Stats")] public List<Stat> EquipStats;
        public List<Stat> UsageStats;
        public Buff_SO[] Buffs;
        public Action_SO Action;
        public List<Action_SO> InventoryActions;
        public List<Action_SO> EquipmentActions;
        public string RequiredAmmoType = string.Empty;
        public GameObject ProvidedAmmo;
        public ItemScript_SO[] Scripts = Array.Empty<ItemScript_SO>();
        public Skill_SO[] Skills = Array.Empty<Skill_SO>();
        public string WeaponType;

        public ItemInstance MakeInstanceWithCount()
        {
            var metadata = ItemMetadataRegistry.Registry[Name];

            var instance = ItemFactory.CreateInstanceFrom(metadata, Count);

            return instance;
        }
    }
}