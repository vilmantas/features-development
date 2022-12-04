using System;
using System.Collections.Generic;
using System.Linq;
using Features.Buffs.UI;
using Features.Character;
using Features.Conditions;
using Features.Equipment.UI;
using Features.Health;
using Features.Inventory.UI;
using Features.Movement;
using Features.Stats.Base;
using Integrations.Actions;
using Integrations.Items;
using Integrations.Skills;
using Integrations.Skills.Actions;
using Integrations.Skills.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.ItemsContainer;

namespace DebugScripts.Character
{
    public class PlayerDebug : MonoBehaviour
    {
        public LayerMask GroundLayer;

        public Player PlayerInstance;

        public InventoryUIController InventoryUI;

        public EquipmentUIController EquipmentUI;

        public BuffUIController BuffUI;

        public StatsUIController StatsUI;

        public ContextMenuUIController ContextMenuUI;

        public HealthUIController HealthUI;

        public StatusEffectsUIController StatusEffectsUI;

        public SkillsUIController SkillsUI;

        public GameObject RootGameObject;
        
        private void Start()
        {
            RootGameObject = transform.root.gameObject;
            
            if (InventoryUI && PlayerInstance.Inventory)
            {
                InventoryUI.Initialize(PlayerInstance.m_InventoryController);
            }

            if (EquipmentUI && PlayerInstance.Equipment)
            {
                EquipmentUI.Initialize(PlayerInstance.m_EquipmentController);
            }

            if (BuffUI && PlayerInstance.Buffs)
            {
                BuffUI.Initialize(PlayerInstance.m_BuffController);
            }

            if (StatsUI && PlayerInstance.Stats)
            {
                StatsUI.Initialize(PlayerInstance.m_StatsController);
            }

            if (HealthUI && PlayerInstance.Health)
            {
                HealthUI.Initialize(PlayerInstance.m_HealthController);
            }
            
            if (PlayerInstance.Inventory)
            {
                PlayerInstance.m_InventoryController.OnContextRequested += ShowContextMenu;
            }

            if (StatusEffectsUI && PlayerInstance.Conditions)
            {
                StatusEffectsUI.Initialize(PlayerInstance.m_StatusEffectsController);
            }

            if (SkillsUI && PlayerInstance.Skills)
            {
                SkillsUI.Initialize(PlayerInstance.m_SkillsController);
            }
        }

        private void ShowContextMenu(StorageData data)
        {
            var item = data.Parent as ItemInstance;

            var inventoryOptions = GetInventoryOptionsFor(item);
            
            ContextMenuUI.Show(Input.mousePosition, inventoryOptions, action =>
            {
                PlayerInstance.m_InventoryController.HandleItemAction(data, action);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) PlayerInstance.m_MovementController.SetRunning(true);

            if (Input.GetKeyUp(KeyCode.LeftShift)) PlayerInstance.m_MovementController.SetRunning(false);
            
            if (Input.GetMouseButtonDown(2)) PlayerInstance.m_CombatController.SetBlocking(true);

            if (Input.GetMouseButtonUp(2)) PlayerInstance.m_CombatController.SetBlocking(false);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var payload = ActivateSkill.MakePayload(RootGameObject, nameof(BasicAttackSkill));

                PlayerInstance.m_ActionsController.DoAction(payload);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject()) return;
                
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
                {
                    var movePayload =
                        Move.MakePayload(RootGameObject, new MoveActionData(hit.point));

                    PlayerInstance.m_ActionsController.DoAction(movePayload);
                }
            }
        }
    }
}