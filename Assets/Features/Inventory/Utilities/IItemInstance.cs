using UnityEngine;

namespace Features.Inventory.Utilities
{
    public interface IItemInstance
    {
        IItemMetadata Metadata { get; }
    }
}