using Features.Inventory;
using Features.Inventory.UI;
using UnityEngine;
using UnityEngine.AI;

namespace DebugScripts.Character
{
    [ExecuteAlways]
    public class PlayerDebug : MonoBehaviour
    {
        public NavMeshAgent NavAgent;

        public LayerMask GroundLayer;

        public InventoryUIController InventoryUI;

        public bool Execute;

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
                {
                    NavAgent.destination = hit.point;
                }
            }

            if (Execute)
            {
                Execute = false;

                Debug.Log("What");

                var inv = GetComponentInChildren<InventoryController>();

                inv.Initialize();

                InventoryUI.Initialize(inv);
            }
        }
    }
}