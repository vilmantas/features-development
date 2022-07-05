using System;

namespace Utilities.ItemsContainer
{
    public class StorageData
    {
        internal readonly Guid Id = Guid.NewGuid();

        public StorageData(object parent, ResourceContainer stackableData)
        {
            Parent = parent;
            StackableData = stackableData;
        }

        public StorageData(object parent)
        {
            Parent = parent;

            StackableData = new ResourceContainer(1, 1);
        }

        public object Parent { get; }

        public ResourceContainer StackableData { get; }

        public T ParentCast<T>() where T : class => Parent as T;

        public override bool Equals(object other)
        {
            if (other is not StorageData b) return false;

            return Parent.Equals(b.Parent);
        }

        public static bool operator !=(StorageData a, StorageData b)
        {
            return a?.Parent != b?.Parent;
        }

        public static bool operator ==(StorageData a, StorageData b)
        {
            return a?.Parent == b?.Parent;
        }

        public override int GetHashCode()
        {
            return Parent.GetHashCode();
        }

        public static StorageData DuplicateEmpty(StorageData original)
        {
            return new StorageData(original.Parent, new ResourceContainer(original.StackableData.Max));
        }
    }
}