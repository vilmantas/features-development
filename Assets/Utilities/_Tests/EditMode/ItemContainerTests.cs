using System;
using System.Linq;
using NUnit.Framework;
using Utilities.ItemsContainer;

namespace ItemContainerTests
{
    public class TestClass : IEquatable<object>
    {
        public TestClass(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool Equals(TestClass other)
        {
            if (other == null) return false;

            return Name.Equals(other.Name);
        }
    }
    public class TestData
    {
        public Func<StorageData, StorageData> FakeFactory =
            (t => new StorageData(t.Parent, t.StackableData.Max));

        public int m_ContainerSize = 5;

        public StorageData SimpleItem = new(new TestClass("Test 1"));

        public StorageData StackableItem = new(new TestClass("Test 2"), 5);
        public Container SUT;
    }

    public class General : TestData
    {
        [SetUp]
        public void SetUp()
        {
            SUT = new Container(FakeFactory, m_ContainerSize);

            SimpleItem = new StorageData(new TestClass("Test 1"));

            StackableItem = new StorageData(new TestClass("Test 2"), 5);
        }

        [Test]
        public void ItHasSize()
        {
            Assert.AreEqual(m_ContainerSize, SUT.Size);
        }

        [Test]
        public void ItHasItems()
        {
            Assert.IsNotNull(SUT.Items);
            Assert.AreEqual(m_ContainerSize, SUT.Slots.Count);
        }

        [Test]
        public void ItCanReceiveAnItem()
        {
            SUT.Add(SimpleItem);

            Assert.AreEqual(SUT.SlotsWithData.First().Item, SimpleItem);
        }

        [Test]
        public void ItAddsDuplicatesIfExistingItemIsFull()
        {
            SUT.Add(SimpleItem, 2);

            Assert.AreEqual(2, SUT.SlotsWithData.Count);

            Assert.AreEqual(SUT.SlotsWithData.First().Item, SUT.SlotsWithData.Skip(1).First().Item);
        }

        [Test]
        public void ItDoesNotAddDuplicateItemsIfExistingItemIsNotFull()
        {
            SUT.Add(StackableItem, 3);

            Assert.AreEqual(1, SUT.SlotsWithData.Count);
        }

        [Test]
        public void ItAppendsToExistingStackableItem()
        {
            SUT.Add(StackableItem, 3);
            SUT.Add(StackableItem, 1);

            Assert.AreEqual(1, SUT.SlotsWithData.Count);
            Assert.AreEqual(4, SUT.SlotsWithData.First().Item.StackableData.Current);
        }

        [Test]
        public void ItDoesNotAppendNewItemToExistingStackable()
        {
            SUT.Add(StackableItem);
            SUT.Add(SimpleItem);

            Assert.AreEqual(SUT.SlotsWithData.First().Item, StackableItem);
            Assert.AreEqual(SUT.SlotsWithData.Skip(1).First().Item, SimpleItem);
        }


        [Test]
        public void ItStacksMultipleItems()
        {
            SUT.Add(StackableItem);
            SUT.Add(SimpleItem);
            SUT.Add(StackableItem);

            Assert.AreEqual(SUT.SlotsWithData.Count, 2);
            Assert.AreEqual(SUT.SlotsWithData.First().Item, StackableItem);
            Assert.AreEqual(SUT.SlotsWithData.First().Item.StackableData.Current, 2);
            Assert.AreEqual(SUT.SlotsWithData.Skip(1).First().Item, SimpleItem);
        }

        [Test]
        public void ItReportsAmountAdded()
        {
            SUT.Add(StackableItem, 6, out int amountAdded);

            Assert.AreEqual(6, amountAdded);
        }

        [Test]
        public void ItCanRemoveItem()
        {
            SUT.Add(SimpleItem);

            SUT.Remove(SimpleItem);

            Assert.IsTrue(SUT.IsEmpty);
        }

        [Test]
        public void ItReportsRemovedAmount()
        {
            SUT.Add(StackableItem, 5);

            SUT.Remove(StackableItem, 3, out int amountRemoved);

            Assert.AreEqual(3, amountRemoved);
        }

        [Test]
        public void ItDoesNotRemoveMoreThanAvailable()
        {
            SUT.Add(StackableItem, 15);

            SUT.Remove(StackableItem, 69, out int amountRemoved);

            Assert.AreEqual(15, amountRemoved);
        }

        [Test]
        public void ItCanRemoveSpecificItem()
        {
            SUT.Add(SimpleItem, 4);

            var target = SUT.SlotsWithData.First().Item;

            Assert.IsTrue(SUT.SlotsWithData.Any(x => ReferenceEquals(x.Item, target)));

            SUT.RemoveExact(target, out int amountRemoved);

            Assert.IsFalse(SUT.SlotsWithData.Any(x => ReferenceEquals(x.Item, target)));
        }

        [Test]
        public void ItCanSwapSlotItems()
        {
            SUT.Add(SimpleItem, 2);

            Assert.AreEqual(2, SUT.SlotsWithData.Count);

            var firstSlot = SUT.SlotsWithData.First();

            var firstSlotItem = SUT.SlotsWithData.First().Item;

            var secondSlot = SUT.SlotsWithData.Skip(1).First();

            var secondSlotItem = SUT.SlotsWithData.Skip(1).First().Item;

            SUT.Swap(firstSlot.Id, secondSlot.Id);

            Assert.AreSame(firstSlotItem, SUT.SlotsWithData.Skip(1).First().Item);
            Assert.AreSame(secondSlotItem, SUT.SlotsWithData.First().Item);
        }

        [Test]
        public void ItCanReplaceExactItem()
        {
            SUT.Add(SimpleItem);

            var originalSlot = SUT.SlotsWithData.First();

            var originalSlotItem = SUT.SlotsWithData.First().Item;

            var replacement = new StorageData(new TestClass("Lol"));

            SUT.ReplaceExact(originalSlot.Id, replacement);

            Assert.AreSame(replacement, originalSlot.Item);
        }
    }

    public class WhenContainerEmpty : TestData
    {
        [SetUp]
        public void SetUp()
        {
            m_ContainerSize = 0;

            SUT = new Container(FakeFactory, m_ContainerSize);
        }
    }
}