using System;
using Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Inventory
{
    public class ReplaceRequest : IChangeRequest
    {
        public StorageData NewItem;
        public Guid OldId;
    }
}