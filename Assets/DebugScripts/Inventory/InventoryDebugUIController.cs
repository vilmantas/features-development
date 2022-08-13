using Features.Inventory;
using Features.Inventory.UI;
using UnityEngine;

namespace DebugScripts
{
    public class InventoryDebugUIController : MonoBehaviour
    {
        public InventoryDebugCharacter Character;

        public BaseInventoryUIData UIPrefab;

        private InventoryUIManager m_Manager;

        private void Start()
        {
            m_Manager = new InventoryUIManager();

            m_Manager.SetSource(Character.GetComponentInChildren<InventoryController>(),
                () =>
                {
                    var instance = Instantiate(UIPrefab.gameObject, transform);
                    return instance.GetComponentInChildren<IInventoryUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }
    }
}