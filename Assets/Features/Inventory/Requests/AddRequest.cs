using Inventory.Abstract.Internal;
using Utilities.ItemsContainer;

namespace Inventory
{
    public class AddRequest : IChangeRequest
    {
        public int Amount;

        public StorageData SourceInventoryItemBase;
    }
}