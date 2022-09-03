using UnityEngine;

namespace Features.Inventory.UI
{
    public class InventoryUIController : MonoBehaviour
    {
        public BaseInventoryUIData InventoryUIDataPrefab;
        private InventoryController m_InventoryController;

        private InventoryUIManager m_UIManager;

        public void Initialize(InventoryController inventory)
        {
            m_InventoryController = inventory;

            m_UIManager = new InventoryUIManager();

            m_UIManager.SetSource(m_InventoryController, CreateMethod, DestroyMethod);
        }

        private IInventoryUIData CreateMethod()
        {
            var instance = Instantiate(InventoryUIDataPrefab.gameObject, transform);

            return instance.GetComponentInChildren<IInventoryUIData>();
        }

        private void DestroyMethod(IInventoryUIData data)
        {
            Destroy(data.gameObject);
        }
    }
}