using System;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public static class ChangeRequestFactory
    {
        public static AddRequest Add(StorageData item, int amount)
        {
            return new AddRequest() {SourceInventoryItemBase = item, Amount = amount};
        }

        public static AddRequest Add(StorageData item)
        {
            return new AddRequest() {SourceInventoryItemBase = item, Amount = item.StackableData.Current};
        }

        public static RemoveRequest Remove(StorageData item, int amount)
        {
            return new RemoveRequest() {SourceInventoryItemBase = item, Amount = amount};
        }

        public static RemoveRequest Remove(StorageData item)
        {
            return new RemoveRequest() {SourceInventoryItemBase = item};
        }

        public static RemoveRequest RemoveExact(StorageData data)
        {
            return new RemoveRequest() {SourceInventoryItemBase = data, RemoveExact = true};
        }

        public static RemoveRequest Remove(AddRequestResult addRequestResult)
        {
            return new RemoveRequest()
            {
                Amount = addRequestResult.AmountAdded,
                SourceInventoryItemBase = addRequestResult.AddRequest.SourceInventoryItemBase
            };
        }

        public static ReplaceRequest Replace(Guid oldId, StorageData newItem)
        {
            return new ReplaceRequest() {OldId = oldId, NewItem = newItem};
        }

        public static SwapSlotsRequest Swap(Guid source, Guid target)
        {
            return new SwapSlotsRequest() {SourceId = source, TargetId = target};
        }
    }
}