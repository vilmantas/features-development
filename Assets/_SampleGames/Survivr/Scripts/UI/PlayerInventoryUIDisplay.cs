using Features.Inventory;
using Features.Inventory.UI;
using UnityEngine;
using Utilities.ItemsContainer;

namespace _SampleGames.Survivr
{
    public class PlayerInventoryUIDisplay : Manager
    {
        private GameObject m_Character;

        private InventoryController m_InventoryController;

        private InventoryUIManager m_UIManager;

        public BaseInventoryUIData InventoryUIDataPrefab;

        public override void Initialize()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player");

            m_InventoryController = m_Character.GetComponentInChildren<InventoryController>();

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