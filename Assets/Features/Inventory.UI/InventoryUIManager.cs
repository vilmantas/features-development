using System;
using System.Collections.Generic;
using Features.Inventory.Abstract.Internal;
using UnityEngine.EventSystems;
using Utilities.ItemsContainer;

namespace Features.Inventory.UI
{
    public class InventoryUIManager
    {
        private Dictionary<Guid, IInventoryUIData> Datas = new();

        private Action<IInventoryUIData> m_DestroyAction;

        private Func<IInventoryUIData> m_InstantiationFunc;
        private InventoryController m_Source;

        public void SetSource(InventoryController controller, Func<IInventoryUIData> instantiationFunc,
            Action<IInventoryUIData> destroyAction)
        {
            UnsubscribeFromSource();

            m_Source = controller;

            m_InstantiationFunc = instantiationFunc;

            m_DestroyAction = destroyAction;

            CreateSlots();

            SubscribeToSource();

            DisplayNewUI();
        }

        private void ClearUI()
        {
            foreach (var data in Datas)
            {
                data.Value.Reset();
            }
        }

        private void DisplayNewUI()
        {
            foreach (var item in m_Source.Slots)
            {
                var data = Datas[item.Id];

                data.SetData(item);
            }
        }

        private void SubscribeToSource()
        {
            m_Source.OnChangeRequestHandled += OnItemChangeRequestHandled;
            m_Source.OnInventoryUpdated += ResetUI;
        }

        private void OnItemChangeRequestHandled(IChangeRequestResult request)
        {
            ResetUI();
        }

        private void ResetUI()
        {
            ClearUI();
            DisplayNewUI();
        }

        private void CreateSlots()
        {
            foreach (var item in m_Source.Slots)
            {
                var data = m_InstantiationFunc();

                data.OnPressed.AddListener(ActivateItem);

                data.OnDragged.AddListener(MoveItem);

                Datas.Add(item.Id, data);
            }
        }

        private void DestroySlots()
        {
            foreach (var data in Datas)
            {
                data.Value.Unsubscribe();

                m_DestroyAction.Invoke(data.Value);
            }

            Datas.Clear();
        }

        private void ActivateItem(PointerEventData.InputButton button, ContainerItem container)
        {
            if (button == PointerEventData.InputButton.Left)
            {
                m_Source.HandleItemAction(container.Item, Constants.DefaultAction);
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

        private void UnsubscribeFromSource()
        {
            if (m_Source == null) return;

            m_Source.OnChangeRequestHandled -= OnItemChangeRequestHandled;
            m_Source.OnInventoryUpdated -= ResetUI;

            DestroySlots();

            m_Source = null;
        }
    }
}