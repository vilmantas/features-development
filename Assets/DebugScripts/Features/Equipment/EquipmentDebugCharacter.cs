using Features.Equipment;
using UnityEngine;
using UnityEngine.Serialization;

namespace DebugScripts.Equipment
{
    public class EquipmentDebugCharacter : MonoBehaviour
    {
        [FormerlySerializedAs("FakeItem")] public DebugItem_SO debugItem;

        public DebugItem_SO Arrows;

        private EquipmentController m_EquipmentController;

        private void Start()
        {
            m_EquipmentController = GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnItemEquipped += ItemEquipped;

            var request = new EquipRequest()
            {
                Slot = "Basef",
                Item = debugItem.GetInstance.DebugEquipmentData
            };

            m_EquipmentController.HandleEquipRequest(request);
        }

        public void EquipArrows()
        {
            var arrows = Arrows.GetInstance;

            arrows.StorageData.StackableData.Receive(55);

            var request = new EquipRequest()
            {
                Slot = "Basef2",
                Item = arrows.DebugEquipmentData,
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