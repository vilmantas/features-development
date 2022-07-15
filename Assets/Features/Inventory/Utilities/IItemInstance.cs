using UnityEngine;

namespace Features.Inventory.Utilities
{
    public interface IItemInstance
    {
        IInventoryItemMetadata Metadata { get; }
    }
}