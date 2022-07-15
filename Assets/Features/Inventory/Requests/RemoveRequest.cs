using System;
using Features.Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class RemoveRequest : IChangeRequest
    {
        public int Amount;
        public Guid Id;

        public StorageData SourceInventoryItemBase;
    }
}