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

        public IReadOnlyList<StorageData> Items => m_Items.Where(x => !x.IsEmpty).Select(x => x.Item).ToList();
        public IReadOnlyList<ContainerItem> Slots => m_Items.ToList();
        public IReadOnlyList<ContainerItem> SlotsWithData => m_Items.Where(x => !x.IsEmpty).ToList();
        public bool IsEmpty => SlotsWithData.Count == 0;

        public void Add(StorageData item)
        {
            Add(item, 1);
        }

        public void Add(StorageData item, int amount)
        {
            Add(item, amount, out var _);
        }

        public void Add(StorageData item, int amount, out int amountAdded)
        {
            Increase(item, amount, out amountAdded);
        }

        public void Swap(ContainerItem first, ContainerItem second)
        {
            SwapSlotItems(first, second);
        }

        public void ReplaceExact(StorageData current, StorageData replacement)
        {
            var slot = Slots.FirstOrDefault(x => x.Item.Id == current.Id);

            if (slot == null) return;

            slot.m_Item = replacement;
        }

        public void RemoveExact(StorageData item)
        {
            RemoveByStorageId(item);
        }

        public void Remove(StorageData item)
        {
            Remove(item, 1);
        }

        public void Remove(StorageData item, int amount)
        {
            Remove(item, amount, out _);
        }

        public void Remove(StorageData item, int amount, out int amountRemoved)
        {
            Reduce(item, amount, out amountRemoved);
        }

        private void Reduce(StorageData item, int amount, out int totalRemoved)
        {
            totalRemoved = 0;

            var amountToRemove = amount;

            while (TryGetItem(item, out var storedItem, true) && amountToRemove > 0)
            {
                var amountRemoved = 0;

                storedItem.StackableData.Reduce(amountToRemove, out var leftovers);

                amountRemoved = amountToRemove - leftovers;

                amountToRemove = leftovers;

                amountToRemove -= amountRemoved;
                totalRemoved += amountRemoved;

                if (!TryGetItem(item, out storedItem, true) || amountToRemove <= 0) break;
            }

            RemoveEmpty();
        }

        private void RemoveEmpty()
        {
            foreach (var containerItem in m_Items.Where(x => !x.IsEmpty && x.Item.StackableData.IsEmpty))
            {
                containerItem.m_Item = null;
            }
        }

        private void RemoveByStorageId(StorageData item)
        {
            var x = SlotsWithData.FirstOrDefault(x => x.Item.Id == item.Id);

            if (x == null) return;

            x.m_Item = null;
        }


        private void Increase(StorageData itemToAdd, int amount, out int amountAdded)
        {
            var amountToAdd = amount;

            AddToExisting(itemToAdd, amount, out amountAdded);

            amountToAdd -= amountAdded;

            if (amountToAdd <= 0) return;

            Fill(itemToAdd, amountToAdd, out var filledAmount);

            amountAdded += filledAmount;
        }

        private void AddToExisting(StorageData itemToAdd, int amount, out int amountAdded)
        {
            amountAdded = 0;

            if (!TryGetItem(itemToAdd, out var item) || item.StackableData.IsFull) return;

            item.StackableData.Receive(amount, out var leftovers);

            amountAdded = amount - leftovers;
        }

        private void Fill(StorageData sourceInventoryItem, int amountToAdd, out int totalAmountAdded)
        {
            var addedCurrently = 0;

            totalAmountAdded = 0;

            while (true)
            {
                if (!TryGetFreeSlot(out var container))
                {
                    totalAmountAdded = addedCurrently;

                    return;
                }

                var newItem = m_ItemFactory(sourceInventoryItem);

                newItem.StackableData.Receive(amountToAdd, out var leftovers);

                addedCurrently += amountToAdd - leftovers;

                amountToAdd = leftovers;

                container.m_Item = newItem;

                if (amountToAdd > 0) continue;

                totalAmountAdded = addedCurrently;

                return;
            }
        }

        private void SwapSlotItems(ContainerItem first, ContainerItem second)
        {
            var source = Slots.FirstOrDefault(x => x.Id == first.Id);

            var target = Slots.FirstOrDefault(x => x.Id == second.Id);

            if (source == null || target == null) return;

            var sourceItem = source.Item;

            var targetItem = target.Item;

            source.m_Item = targetItem;

            target.m_Item = sourceItem;
        }

        private bool TryGetFreeSlot(out ContainerItem containerItem)
        {
            containerItem = m_Items.FirstOrDefault(x => x.IsEmpty);

            return containerItem != null;
        }

        private bool TryGetItem(StorageData item, out StorageData inventoryItem, bool ignoreEmpty = false)
        {
            var items =
                SlotsWithData.Select(x => x.Item);

            if (ignoreEmpty)
            {
                items = items.Where(x => x.StackableData.HasStuff);
            }

            items = items.ToList();

            if (items.Count() > 1 && items.Any(x => x.StackableData.CanReceive))
            {
                items = items.Where(x => x.StackableData.CanReceive).ToList();
            }

            inventoryItem = items.FirstOrDefault();

            return inventoryItem != null;
        }
    }
}