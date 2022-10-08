using Features.Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Features.Inventory.Requests
{
    public class RemoveExactRequest : ChangeRequest
    {
        public StorageData Item;
    }
}