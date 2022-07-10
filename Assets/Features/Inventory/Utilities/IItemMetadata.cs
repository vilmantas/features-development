using UnityEngine;

namespace Features.Inventory.Utilities
{
    public interface IItemMetadata
    {
        string Name { get; }
        Sprite Sprite { get; }
        int MaxStack { get; }
    }
}