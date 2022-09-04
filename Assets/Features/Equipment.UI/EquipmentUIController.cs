using UnityEngine;

namespace Features.Equipment.UI
{
    public class EquipmentUIController : MonoBehaviour
    {
        public BaseEquipmentUIData EquipmentUIDataPrefab;
        private EquipmentController m_EquipmentController;

        private EquipmentUIManager m_UIManager;

        public void Initialize(EquipmentController equipment)
        {
            m_EquipmentController = equipment;

            m_UIManager = new EquipmentUIManager();

            m_UIManager.SetSource(m_EquipmentController, CreateMethod, DestroyMethod);
        }

        private IEquipmentUIData CreateMethod()
        {
            var instance = Instantiate(EquipmentUIDataPrefab.gameObject, transform);

            return instance.GetComponentInChildren<IEquipmentUIData>();
        }

        private void DestroyMethod(IEquipmentUIData data)
        {
            Destroy(data.gameObject);
        }
    }
}