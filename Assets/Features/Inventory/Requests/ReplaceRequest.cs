using System;
using Features.Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class ReplaceRequest : IChangeRequest
    {
        public StorageData NewItem;
        public Guid OldId;
    }
}