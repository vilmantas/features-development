using System;
using System.Collections.Generic;
using System.Linq;
using Features.Inventory.Abstract.Internal;
using Features.Inventory.Events;
using Features.Inventory.Requests;
using UnityEngine;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Range(1, 50)] public int Size = 20;

        public Action<IChangeRequestResult> OnChangeRequestHandled;

        public Action<StorageData> OnContextRequested;

        private Container m_Container;

        public Action<StorageData, string> OnActionSelected;

        public Action OnInventoryUpdated;

        public IEnumerable<StorageData> Items => m_Container.Items;

        public IEnumerable<ContainerItem> Slots => m_Container.Slots;

        public bool HasEmptySpace => !m_Container.IsFull;

        public bool IsFull => m_Container.IsFull;

        public void Awake()
        {
            m_Container = new Container(InventoryItemFactories.MakeItem, Size);
        }

        public void Initialize(int size)
        {
            Size = size;

            m_Container = new Container(InventoryItemFactories.MakeItem, Size);
        }

        public void NotifyChange()
        {
            OnInventoryUpdated?.Invoke();
        }

        public bool CanReceive(StorageData request, out int maxAmount)
        {
            maxAmount = 0;

            if (IsFull && request.StackableData.Max == 1) return false;

            var stackSize = request.StackableData.Max;

            maxAmount = m_Container.SlotsWithoutData.Count * stackSize;

            var existingItems = m_Container.SlotsWithItem(request);

            maxAmount += existingItems.Sum(x => x.Item.Max - x.Item.Current);

            return maxAmount != 0;
        }

        public IChangeRequestResult HandleRequest(IChangeRequest request)
        {
            IChangeRequestResult result = null;

            switch (request)
            {
                case AddRequest addRequest:
                    m_Container.Add(addRequest.Item, addRequest.Amount, out var amountAdded);

                    result = new AddRequestResult(addRequest, amountAdded > 0, amountAdded);
                    break;
                case RemoveExactRequest removeExactRequest:
                    m_Container.RemoveExact(removeExactRequest.Item, out int exactAmountRemoved);

                    break;
                case RemoveRequest removeRequest:
                    m_Container.Remove(removeRequest.Item, removeRequest.Amount,
                        out int amountRemoved);

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

            OnChangeRequestHandled?.Invoke(result);

            return result;
        }

        public void HandleItemAction(ContainerItem container, string action)
        {
            OnActionSelected?.Invoke(container.Item, action);
        }

        public void HandleItemContextOpen(ContainerItem container)
        {
            OnContextRequested?.Invoke(container.Item);
        }
    }
}