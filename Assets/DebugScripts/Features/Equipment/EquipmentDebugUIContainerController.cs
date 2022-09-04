using Features.Equipment;
using Features.Equipment.UI;
using UnityEngine;

namespace DebugScripts.Equipment
{
    public class EquipmentDebugUIContainerController : MonoBehaviour
    {
        public EquipmentDebugCharacter Character;

        public BaseEquipmentUIData UIPrefab;

        private EquipmentUIManager m_UIManager;

        private void Start()
        {
            m_UIManager = new EquipmentUIManager();

            m_UIManager.SetSource(Character.GetComponentInChildren<EquipmentController>(),
                () =>
                {
                    var instance = Instantiate(UIPrefab.gameObject, transform);
                    return instance.GetComponentInChildren<IEquipmentUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }
    }
}