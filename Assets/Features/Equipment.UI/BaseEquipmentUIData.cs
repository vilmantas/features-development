using Features.Equipment.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Equipment
{
    public class BaseEquipmentUIData : MonoBehaviour, IPointerClickHandler, IEquipmentUIData
    {
        private readonly EquipmentButtonPressEvent m_EquipmentButtonPressEvent = new();

        private Image Blocker;

        private EquipmentContainerItem m_EquipmentContainer;

        private void Awake()
        {
            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.name.EndsWith("blocker"))
                {
                    Blocker = image;
                }
            }

            OnAwake();
        }

        public void SetData(EquipmentContainerItem item)
        {
            m_EquipmentContainer = item;

            name = item.Slot;

            Blocker.enabled = item.IsEmpty;

            OnSetData(item);
        }

        public EquipmentButtonPressEvent OnPressed => m_EquipmentButtonPressEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_EquipmentContainer == null || m_EquipmentContainer.IsEmpty) return;

            OnPressed.Invoke(m_EquipmentContainer);
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnSetData(EquipmentContainerItem item)
        {
        }
    }
}