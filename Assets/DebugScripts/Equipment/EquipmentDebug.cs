using Equipment;
using Equipment.Unity;
using UnityEngine;

namespace DebugScripts.Equipment
{
    public class EquipmentDebug : MonoBehaviour
    {
        public FakeItem_SO FakeItem;
        private EquipmentController m_EquipmentController;

        private void Start()
        {
            m_EquipmentController = GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnItemEquippedEvent.AddListener(ItemEquipped);

            var request = new EquipRequest()
            {
                SlotType = "Basef",
                Item = FakeItem.GetInstance.EquipmentData
            };

            m_EquipmentController.HandleEquipRequest(request);
        }

        private void ItemEquipped(EquipResult arg0)
        {
            print(arg0.Succeeded);
        }
    }
}