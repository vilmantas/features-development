using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Abstract.Internal;
using UnityEngine.EventSystems;
using Utilities.ItemsContainer;

namespace Features.Inventory.UI
{
    public class InventoryUIManager
    {
        private List<IInventoryUIData> Datas = new();

        private Action<IInventoryUIData> m_DestroyAction;

        private Func<IInventoryUIData> m_InstantiationFunc;
        private InventoryController m_Source;

        public void SetSource(InventoryController controller, Func<IInventoryUIData> instantiationFunc,
            Action<IInventoryUIData> destroyAction)
        {
            m_Source = controller;

            m_InstantiationFunc = instantiationFunc;

            m_DestroyAction = destroyAction;

            SubscribeToSource();

            DisplayNewUI();
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
                m_Source.HandleItemAction(container, Constants.DefaultAction);
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
            m_Source.OnInventoryUpdated += ResetUI;
        }
        
        private void ResetUI()
        {
            ClearUI();
            DisplayNewUI();
        }

        private void UnsubscribeFromSource()
        {
            m_Source.OnChangeRequestHandled.RemoveListener(OnItemChangeRequestHandled);
        }
    }
}