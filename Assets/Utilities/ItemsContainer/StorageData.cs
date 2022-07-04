using System;

namespace Utilities.ItemsContainer
{
    public class StorageData : IEquatable<StorageData>
    {
        public StorageData(object parent, ResourceContainer stackableData)
        {
            Parent = parent;
            StackableData = stackableData;
        }

        object Parent { get; }

        ResourceContainer StackableData { get; }

        public bool IsStackable => StackableData != null;

        public new bool Equals(StorageData other)
        {
            if (other is not { } b) return false;

            return Parent.Equals(b.Parent);
        }

        T ParentCast<T>() where T : class => Parent as T;

        public override int GetHashCode()
        {
            return HashCode.Combine(StackableData, Parent);
        }
    }
}