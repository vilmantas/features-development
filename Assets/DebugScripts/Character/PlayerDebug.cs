using System;
using System.Collections.Generic;
using System.Linq;
using Features.Actions;
using Features.Buffs.UI;
using Features.Character;
using Features.Equipment.UI;
using Features.Health;
using Features.Inventory.UI;
using Features.Items;
using Features.Stats.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utilities.ItemsContainer;

namespace DebugScripts.Character
{
    public class PlayerDebug : MonoBehaviour
    {
        public NavMeshAgent NavAgent;

        public LayerMask GroundLayer;

        public Player PlayerInstance;

        public InventoryUIController InventoryUI;

        public EquipmentUIController EquipmentUI;

        public BuffUIController BuffUI;

        public StatsUIController StatsUI;

        public ContextMenuUIController ContextMenuUI;

        public HealthUIController HealthUI;

        private void Start()
        {
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
        }

        private void ShowContextMenu(StorageData data)
        {
            var item = data.Parent as ItemInstance;

            var inventoryOptions = GetInventoryOptionsFor(item);
            
            ContextMenuUI.Show(Input.mousePosition, inventoryOptions, s => DoAction(s, item, PlayerInstance.m_ActionsController));
        }

        private static List<string> GetInventoryOptionsFor(ItemInstance item)
        {
            var options = item.Metadata.InventoryContextMenuActions
                .Select(x => x.DisplayName)
                .Prepend(item.Metadata.Action.DisplayName)
                .Append("Drop");

            return options.ToList();
        }

        private static void DoAction(string action, ItemInstance item, ActionsController controller)
        {
            var actionBase = new ActionBase(action);
            
            var actionPayload = new ActionActivationPayload(actionBase, item, controller.transform.root.gameObject);

            controller.DoAction(actionPayload);
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject()) return;
                
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
                {
                    NavAgent.destination = hit.point;
                }
            }
        }
    }
}