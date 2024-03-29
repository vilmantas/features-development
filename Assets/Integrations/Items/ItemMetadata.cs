using System;
using System.Collections.Generic;
using Features.Actions;
using Features.Buffs;
using Features.Equipment;
using Features.Inventory.UI.Utilities;
using Features.Skills;
using Features.Stats.Base;
using Features.WeaponAnimationConfigurations;
using UnityEngine;

namespace Integrations.Items
{
    public class ItemMetadata : IInventoryItemMetadata, IEquipmentItemMetadata
    {
        public readonly Guid Id = Guid.NewGuid();

        public ItemMetadata(string name, Sprite sprite, int maxStack, StatGroup equipStats,
            string mainSlot,
            string secondarySlot, GameObject modelPrefab, List<BuffMetadata> buffs, ActionBase action,
            List<ActionBase> inventoryContextMenuActions,
            List<ActionBase> equipmentContextMenuActions,
            StatGroup usageStats,
            string requiredAmmo,
            GameObject providedAmmo,
            ItemScriptDTO[] scripts,
            SkillMetadata[] skills,
            string weaponType,
            WeaponAnimationsDTO animations)
        {
            Name = name;
            Sprite = sprite;
            MaxStack = maxStack < 1 ? 1 : maxStack;
            EquipStats = equipStats;
            MainSlot = mainSlot;
            SecondarySlot = secondarySlot;
            IsStackable = MaxStack > 1;
            ModelPrefab = modelPrefab;
            Buffs = buffs;
            Action = action;
            InventoryContextMenuActions = inventoryContextMenuActions;
            EquipmentContextMenuActions = equipmentContextMenuActions;
            UsageStats = usageStats;
            RequiredAmmo = requiredAmmo;
            ProvidedAmmo = providedAmmo;
            Scripts = scripts;
            Skills = skills;
            WeaponType = weaponType;
            WeaponAnimations = animations;
        }

        public string RequiredAmmo { get; }
        public GameObject ProvidedAmmo { get; }
        public StatGroup EquipStats { get; }
        public StatGroup UsageStats { get; }
        public List<BuffMetadata> Buffs { get; }
        public ActionBase Action { get; }
        public List<ActionBase> InventoryContextMenuActions;
        public List<ActionBase> EquipmentContextMenuActions;
        public GameObject ModelPrefab { get; }
        public string MainSlot { get; }
        public string SecondarySlot { get; }
        public bool IsStackable { get; }
        public string Name { get; }
        public Sprite Sprite { get; }
        public int MaxStack { get; }
        public ItemScriptDTO[] Scripts { get; }
        public SkillMetadata[] Skills { get; }
        public string WeaponType { get; }
        public WeaponAnimationsDTO WeaponAnimations { get; }
    }
}