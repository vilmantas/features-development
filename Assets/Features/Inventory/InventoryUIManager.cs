using System;
using System.Collections.Generic;
using Inventory;
using Inventory.Abstract.Internal;
using UnityEngine.EventSystems;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class InventoryUIManager
    {
        private List<IInventorySlotUI> Datas = new();

        private Action<IInventorySlotUI> m_DestroyAction;

        private Func<IInventorySlotUI> m_InstantiationFunc;
        private InventoryController m_Source;

        public void SetSource(InventoryController controller, Func<IInventorySlotUI> instantiationFunc,
            Action<IInventorySlotUI> destroyAction)
        {
            m_Source = controller;

            m_InstantiationFunc = instantiationFunc;

            m_DestroyAction = destroyAction;

            SubscribeToSource();
        }

        private void OnItemChangeRequestHandled(IChangeRequestResult request)
        {
            ClearUI();
            DisplayNewUI();
        }

        private void ClearUI()
        {
            foreach (var data in Datas)
            {
                m_DestroyAction(data);
            }

            Datas.Clear();
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_Source.Slots)
            {
                var data = m_InstantiationFunc();

                data.OnPressed.AddListener(ActivateItem);

                data.OnDragged.AddListener(MoveItem);

                data.SetData(item);

                Datas.Add(data);
            }
        }

        private void ActivateItem(PointerEventData.InputButton button, ContainerItem container)
        {
            if (button == PointerEventData.InputButton.Left)
            {
                m_Source.HandleItemAction(container, "Click");
            }
            else
            {
                m_Source.HandleItemContextOpen(container);
            }
        }

        private void MoveItem(ContainerItem transferredItem, ContainerItem target)
        {
            m_Source.HandleRequest(ChangeRequestFactory.Swap(transferredItem.Id, target.Id));
        }

        private void SubscribeToSource()
        {
            m_Source.OnChangeRequestHandled.AddListener(OnItemChangeRequestHandled);
        }

        private void UnsubscribeFromSource()
        {
            m_Source.OnChangeRequestHandled.RemoveListener(OnItemChangeRequestHandled);
        }
    }
}