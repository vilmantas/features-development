using Features.Equipment;
using Features.Equipment.UI;
using UnityEngine;

namespace DebugScripts.Equipment
{
    public class EquipmentDebug : MonoBehaviour
    {
        public Transform UIContainer;

        public BaseEquipmentUIData UIPrefab;

        public FakeItem_SO FakeItem;

        public FakeItem_SO Arrows;

        private EquipmentController m_EquipmentController;

        private void Start()
        {
            m_EquipmentController = GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnItemEquipped += ItemEquipped;

            m_EquipmentController.OnItemUnequipRequested += HandleUnequipRequest;

            var request = new EquipRequest()
            {
                SlotType = "Basef",
                ItemInstance = FakeItem.GetInstance.EquipmentData
            };

            m_EquipmentController.HandleEquipRequest(request);
        }

        private void HandleUnequipRequest(EquipmentContainerItem arg0)
        {
            var req = new EquipRequest()
            {
                SlotType = arg0.Slot,
                ItemInstance = null
            };

            m_EquipmentController.HandleEquipRequest(req);
        }

        public void EquipArrows()
        {
            var arrows = Arrows.GetInstance;

            arrows.StorageData.StackableData.Receive(55);

            var request = new EquipRequest()
            {
                SlotType = "Basef2",
                ItemInstance = arrows.EquipmentData,
            };

            m_EquipmentController.HandleEquipRequest(request);
        }

        private void ItemEquipped(EquipResult arg0)
        {
            if (!arg0.Succeeded) return;

            print(arg0.EquipmentContainerItem.IsEmpty
                ? $"Item unequipped from slot: {arg0.EquipmentContainerItem.Slot}"
                : $"Item equipped to slot: {arg0.EquipmentContainerItem.Slot}");
        }
    }
}