using System;
using Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Inventory
{
    public class RemoveRequest : IChangeRequest
    {
        public int Amount;
        public Guid Id;

        public StorageData SourceInventoryItemBase;
    }
}