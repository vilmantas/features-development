using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.UI;
using UnityEngine;
using Utilities.ItemsContainer;

namespace DebugScripts
{
    public class InventoryDebugCharacter : MonoBehaviour
    {
        public Transform UIContainer;

        public DebugItem_SO Arrows;

        public DebugItem_SO Axe;

        public DebugItem_SO Pickaxe;

        public BaseInventoryUIData baseInventoryUIPrefab;

        private InventoryController m_InventoryController;

        private void Start()
        {
            InventoryItemFactories.Register(typeof(DebugItemInstance), wtf);

            m_InventoryController = GetComponentInChildren<InventoryController>();

            m_InventoryController.OnChangeRequestHandled.AddListener(ChangeRequestHandled);

            // m_InventoryController.WithUI(baseInventoryUIPrefab, UIContainer.transform);
        }

        private StorageData wtf(StorageData arg)
        {
            var itemInstance = arg.Parent as DebugItemInstance;

            return new DebugItemInstance(itemInstance.Metadata).StorageData;
        }

        private void ChangeRequestHandled(IChangeRequestResult arg0)
        {
        }

        public void GiveAxe()
        {
            var request = ChangeRequestFactory.Add(Axe.GetInstance.StorageData, 1);

            m_InventoryController.HandleRequest(request);
        }

        public void Give5Axes()
        {
            var request = ChangeRequestFactory.Add(Pickaxe.GetInstance.StorageData, 5);

            m_InventoryController.HandleRequest(request);
        }

        public void GiveRandomArrows()
        {
            var request = ChangeRequestFactory.Add(Arrows.GetInstance.StorageData, Random.Range(50, 100));

            m_InventoryController.HandleRequest(request);
        }
    }
}