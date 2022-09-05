using Features.Buffs.UI;
using Features.Character;
using Features.Equipment.UI;
using Features.Inventory.UI;
using Features.Stats.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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