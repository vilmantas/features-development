using System;
using Features.Inventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class BaseInventoryUIData : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler,
        IInventoryUIData
    {
        [HideInInspector] public InventoryButtonPressEvent m_OnPressed = new();

        [HideInInspector] public InventorySlotDraggedEvent m_OnDragged = new();

        private Image Blocker;

        private bool IsDragged = false;
        protected ContainerItem m_InventoryItemContainer { get; private set; }

        private void Awake()
        {
            var images = GetComponentsInChildren<Image>();

            foreach (var image in images)
            {
                if (image.name.EndsWith("blocker"))
                {
                    Blocker = image;
                    break;
                }
            }

            OnAwake();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_InventoryItemContainer.IsEmpty) return;

            IsDragged = true;

            transform.localScale = Vector3.one;

            Blocker.enabled = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (m_InventoryItemContainer.IsEmpty) return;

            IsDragged = false;

            Blocker.enabled = false;

            transform.localScale = Vector3.one;

            if (eventData.pointerEnter == null) return;

            var target = eventData.pointerEnter.GetComponentInParent<BaseInventoryUIData>();

            if (target == null) return;

            OnDragged.Invoke(m_InventoryItemContainer, target.m_InventoryItemContainer);
        }

        public void SetData(ContainerItem data)
        {
            m_InventoryItemContainer = data;

            Blocker.enabled = data.IsEmpty;

            OnSetData(data);
        }

        public InventoryButtonPressEvent OnPressed => m_OnPressed;
        public InventorySlotDraggedEvent OnDragged => m_OnDragged;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_InventoryItemContainer.IsEmpty) return;

            OnPressed.Invoke(eventData.button, m_InventoryItemContainer);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsDragged) return;
            if (m_InventoryItemContainer.Item == null && !eventData.dragging) return;

            if (eventData.dragging)
            {
                var source = eventData.pointerDrag.GetComponentInParent<BaseInventoryUIData>();

                if (source == null || source.m_InventoryItemContainer.IsEmpty) return;
            }

            transform.localScale = Vector3.one * 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnSetData(ContainerItem data)
        {
        }
    }
}


[Serializable]
public class InventoryButtonPressEvent : UnityEvent<PointerEventData.InputButton, ContainerItem>
{
}

[Serializable]
public class InventorySlotDraggedEvent : UnityEvent<ContainerItem, ContainerItem>
{
}