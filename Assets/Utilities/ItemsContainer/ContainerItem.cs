using System;

namespace Utilities.ItemsContainer
{
    public class ContainerItem
    {
        public readonly Guid Id = Guid.NewGuid();
        public StorageData Item;
        public bool IsEmpty => Item == null;
    }
}