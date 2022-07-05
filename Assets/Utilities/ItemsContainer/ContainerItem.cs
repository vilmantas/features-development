using System;

namespace Utilities.ItemsContainer
{
    public class ContainerItem
    {
        public readonly Guid Id = Guid.NewGuid();

        internal StorageData m_Item;
        public bool IsEmpty => Item == null;
        public StorageData Item => m_Item;
    }
}