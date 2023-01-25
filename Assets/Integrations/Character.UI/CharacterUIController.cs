using System;
using System.Collections.Generic;
using System.Linq;
using Features.Buffs.UI;
using Features.Character;
using Features.Conditions;
using Features.Equipment.UI;
using Features.Health;
using Features.Inventory.UI;
using Features.Stats.Base;
using Integrations.Items;
using Integrations.Skills.UI;
using Managers;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Integrations.Character.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        private Player m_Player;

        private InventoryUIController InventoryUI;

        private EquipmentUIController EquipmentUI;

        private BuffUIController BuffUI;

        private StatsUIController StatsUI;

        private ContextMenuUIController ContextMenuUI;

        private HealthUIController HealthUI;

        private StatusEffectsUIController StatusEffectsUI;

        private SkillsUIController SkillsUI;

        private void Awake()
        {
            InventoryUI = GetComponentInChildren<InventoryUIController>();
            EquipmentUI = GetComponentInChildren<EquipmentUIController>();
            BuffUI = GetComponentInChildren<BuffUIController>();
            StatsUI = GetComponentInChildren<StatsUIController>();
            ContextMenuUI = GetComponentInChildren<ContextMenuUIController>();
            HealthUI = GetComponentInChildren<HealthUIController>();
            StatusEffectsUI = GetComponentInChildren<StatusEffectsUIController>();
            SkillsUI = GetComponentInChildren<SkillsUIController>();
        }

        private void Start()
        {
            m_Player = GameplayManager.Player;
            
            if (InventoryUI && m_Player.Inventory)
            {
                InventoryUI.Initialize(m_Player.m_InventoryController);
            }

            if (EquipmentUI && m_Player.Equipment)
            {
                EquipmentUI.Initialize(m_Player.m_EquipmentController);
            }

            if (BuffUI && m_Player.Buffs)
            {
                BuffUI.Initialize(m_Player.m_BuffController);
            }

            if (StatsUI && m_Player.Stats)
            {
                StatsUI.Initialize(m_Player.m_StatsController);
            }

            if (HealthUI && m_Player.Health)
            {
                HealthUI.Initialize(m_Player.m_HealthController);
            }
            
            if (m_Player.Inventory)
            {
                m_Player.m_InventoryController.OnContextRequested += ShowContextMenu;
            }

            if (StatusEffectsUI && m_Player.Conditions)
            {
                StatusEffectsUI.Initialize(m_Player.m_StatusEffectsController);
            }

            if (SkillsUI && m_Player.Skills)
            {
                SkillsUI.Initialize(m_Player.m_SkillsController,
                    m_Player.m_CooldownsController, m_Player.m_ChannelingController);
            }
        }
        
        
        private void ShowContextMenu(StorageData data)
        {
            var item = data.Parent as ItemInstance;

            var inventoryOptions = GetInventoryOptionsFor(item);
            
            ContextMenuUI.Show(Input.mousePosition, inventoryOptions, action =>
            {
                m_Player.m_InventoryController.HandleItemAction(data, action);
            });
        }
        
        private static List<string> GetInventoryOptionsFor(ItemInstance item)
        {
            var options = item.Metadata.InventoryContextMenuActions
                .Select(x => x.DisplayName)
                .Prepend(item.Metadata.Action.DisplayName)
                .Append("Drop");

            return options.ToList();
        }
    }
}