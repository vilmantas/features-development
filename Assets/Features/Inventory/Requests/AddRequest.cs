using Features.Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Features.Inventory
{
    public class AddRequest : ChangeRequest
    {
        public int Amount;

        public StorageData Item;
    }
}