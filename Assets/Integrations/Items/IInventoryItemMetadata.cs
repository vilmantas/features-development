using UnityEngine;

namespace Features.Inventory.UI.Utilities
{
    public interface IInventoryItemMetadata
    {
        string Name { get; }
        Sprite Sprite { get; }
        int MaxStack { get; }
    }
}