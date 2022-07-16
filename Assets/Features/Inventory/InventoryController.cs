using System;
using System.Collections.Generic;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Events;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Range(1, 50)] public int Size = 20;

        public readonly ChangeRequestHandledEvent OnChangeRequestHandled = new();

        public readonly ContextRequestEvent OnContextRequested = new();

        private Container m_Container;

        public Action<StorageData, string> OnActionSelected;

        private InventoryUIManager UIManager;

        public IEnumerable<StorageData> Items => m_Container.Items;

        public IEnumerable<ContainerItem> Slots => m_Container.Slots;

        public bool HasEmptySpace => !m_Container.IsFull;

        public void Awake()
        {
            UIManager = new InventoryUIManager();

            m_Container = new Container(InventoryItemFactories.MakeItem, Size);
        }

        public void WithUI(IInventoryUIData prefab, Transform container)
        {
            UIManager.SetSource(this,
                () =>
                {
                    var instance = Instantiate(prefab.gameObject, container);
                    return instance.GetComponentInChildren<IInventoryUIData>();
                },
                controller => DestroyImmediate(controller.gameObject));
        }

        public IChangeRequestResult HandleRequest(IChangeRequest request)
        {
            IChangeRequestResult result = null;

            switch (request)
            {
                case AddRequest addRequest:
                    m_Container.Add(addRequest.SourceInventoryItemBase, addRequest.Amount, out var amountAdded);

                    result = new AddRequestResult(addRequest, amountAdded > 0, amountAdded);
                    break;
                case RemoveRequest removeRequest:
                    int amountRemoved;

                    if (removeRequest.RemoveExact)
                    {
                        m_Container.RemoveExact(removeRequest.SourceInventoryItemBase, out amountRemoved);
                    }
                    else
                    {
                        m_Container.Remove(removeRequest.SourceInventoryItemBase, removeRequest.Amount,
                            out amountRemoved);
                    }


                    result = new RemoveRequestResult(removeRequest, amountRemoved > 0, amountRemoved);
                    break;
                case ReplaceRequest replaceRequest:
                    var replaceSuccess = m_Container.ReplaceExact(replaceRequest.OldId, replaceRequest.NewItem);

                    result = new ReplaceRequestResult(replaceRequest, replaceSuccess);
                    break;
                case SwapSlotsRequest swapSlotsRequest:
                    var swapSuccess = m_Container.Swap(swapSlotsRequest.SourceId, swapSlotsRequest.TargetId);

                    result = new SwapSlotsRequestResult(swapSlotsRequest, swapSuccess);
                    break;

                default:
                    Debug.LogError($"Unknown message received of type {nameof(request)}");
                    return null;
            }

            OnChangeRequestHandled.Invoke(result);

            return result;
        }

        public void HandleItemAction(ContainerItem container, string action)
        {
            OnActionSelected?.Invoke(container.Item, action);
        }

        public void HandleItemContextOpen(ContainerItem container)
        {
            OnContextRequested.Invoke(container.Item);
        }
    }
}