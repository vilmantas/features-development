using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.ItemsContainer
{
    public class Container
    {
        private readonly Func<StorageData, StorageData> m_ItemFactory;

        private readonly List<ContainerItem> m_Items;
        public readonly int Size;

        public Container(Func<StorageData, StorageData> itemFactory, int size)
        {
            m_ItemFactory = itemFactory;
            Size = size;

            m_Items = new List<ContainerItem>(size);

            for (var i = 0; i < size; i++)
            {
                m_Items.Add(new ContainerItem());
            }
        }

        public IReadOnlyList<StorageData> Items => m_Items.Select(x => x.Item).ToList();
        public IReadOnlyList<ContainerItem> SlotsWithData => m_Items.Where(x => !x.IsEmpty).ToList();

        public void Add(StorageData item)
        {
            Add(item, 1);
        }

        public void Add(StorageData item, int amount)
        {
            Increase(item, amount);
        }

        private void Increase(StorageData itemToAdd, int amount)
        {
            var amountToAdd = amount;

            while (true)
            {
                if (!TryGetFreeSlot(out var container))
                {
                    return;
                }

                var newItem = m_ItemFactory(itemToAdd);

                const int amountAdded = 1;

                amountToAdd -= amountAdded;

                container.Item = newItem;

                if (amountToAdd > 0) continue;

                return;
            }
        }

        private bool TryGetFreeSlot(out ContainerItem containerItem)
        {
            containerItem = m_Items.FirstOrDefault(x => x.IsEmpty);

            return containerItem != null;
        }
    }
}