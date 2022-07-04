using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Utilities.ItemsContainer;

namespace ItemContainerTests
{
    public class TestData
    {
        public Func<StorageData, StorageData> FakeFactory = (t => t);

        public StorageData Item = new Mock<StorageData>().Object;

        public int m_ContainerSize = 5;
        public Container SUT;
    }

    public class General : TestData
    {
        [SetUp]
        public void SetUp()
        {
            SUT = new Container(FakeFactory, m_ContainerSize);
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
            Assert.AreEqual(m_ContainerSize, SUT.Items.Count);
        }

        [Test]
        public void ItCanReceiveAnItem()
        {
            SUT.Add(Item);

            Assert.AreEqual(SUT.Items.First(), Item);
        }

        [Test]
        public void ItCanReceiveItems()
        {
            SUT.Add(Item, 2);

            Assert.AreEqual(SUT.Items.First(), Item);
            Assert.AreEqual(SUT.Items.Skip(1).First(), Item);
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