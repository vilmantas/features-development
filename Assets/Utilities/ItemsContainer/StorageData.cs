using System;

namespace Utilities.ItemsContainer
{
    public class StorageData
    {
        internal readonly Guid Id = Guid.NewGuid();

        public StorageData(object parent, int maxStackSize = 1)
        {
            Parent = parent;
            StackableData = new ResourceContainer(maxStackSize);
        }

        public StorageData(StorageData original)
        {
            Parent = original.Parent;

            StackableData = new ResourceContainer(original.StackableData.Max);
        }

        public object Parent { get; }

        public ResourceContainer StackableData { get; }

        public T ParentCast<T>() where T : class => Parent as T;

        public override bool Equals(object other)
        {
            if (other is not StorageData b) return false;

            return Parent.Equals(b.Parent);
        }

        public override int GetHashCode()
        {
            return Parent.GetHashCode();
        }
    }
}