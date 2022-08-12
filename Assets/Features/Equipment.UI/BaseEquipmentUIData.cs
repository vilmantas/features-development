using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Equipment.UI
{
    public class BaseEquipmentUIData : MonoBehaviour, IPointerClickHandler, IEquipmentUIData
    {
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

        public Action<EquipmentContainerItem> OnPressed { get; set; }


        public void SetData(EquipmentContainerItem item)
        {
            m_EquipmentContainer = item;

            name = item.Slot;

            Blocker.enabled = item.IsEmpty;

            OnSetData(item);
        }

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