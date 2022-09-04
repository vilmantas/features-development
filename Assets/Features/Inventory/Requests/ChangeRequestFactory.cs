using System;
using Features.Inventory.Requests;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public static class ChangeRequestFactory
    {
        public static AddRequest Add(IEquatable<object> item, int amount)
        {
            return new AddRequest() {Item = new StorageData(item), Amount = amount};
        }

        public static AddRequest Add(StorageData item, int amount)
        {
            return new AddRequest() {Item = item, Amount = amount};
        }

        public static AddRequest Add(StorageData item)
        {
            return new AddRequest() {Item = item, Amount = item.Current};
        }

        public static RemoveRequest Remove(StorageData item, int amount)
        {
            return new RemoveRequest() {Item = item, Amount = amount};
        }

        public static RemoveRequest Remove(StorageData item)
        {
            return new RemoveRequest() {Item = item, Amount = item.Current};
        }

        public static RemoveExactRequest RemoveExact(StorageData data)
        {
            return new RemoveExactRequest() {Item = data};
        }

        public static RemoveRequest Remove(AddRequestResult addRequestResult)
        {
            return new RemoveRequest()
            {
                Amount = addRequestResult.AmountAdded,
                Item = addRequestResult.AddRequest.Item
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