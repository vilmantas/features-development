using Equipment;
using Equipment.Unity;
using UnityEngine;

namespace DebugScripts.Equipment
{
    public class EquipmentDebug : MonoBehaviour
    {
        private EquipmentController m_EquipmentController;

        private void Start()
        {
            m_EquipmentController = GetComponentInChildren<EquipmentController>();

            m_EquipmentController.OnItemEquippedEvent.AddListener(ItemEquipped);

            var request = new EquipRequest()
            {
                SlotType = "Basef"
            };

            m_EquipmentController.HandleEquipRequest(request);
        }

        private void ItemEquipped(EquipResult arg0)
        {
            print(arg0.Succeeded);
        }
    }
}