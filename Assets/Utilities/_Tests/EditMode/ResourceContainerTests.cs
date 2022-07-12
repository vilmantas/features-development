using NUnit.Framework;
using Utilities;

namespace ResourceContainerTests
{
    public class TestData
    {
        protected int m_currentAmount;

        protected int m_Max;
        protected ResourceContainer m_SUT;

        public void Initialize()
        {
            m_SUT = new ResourceContainer(m_Max, m_currentAmount);
        }
    }

    public class General : TestData
    {
        [SetUp]
        public void Setup()
        {
            m_currentAmount = 5;
            m_Max = 10;

            Initialize();
        }

        [Test]
        public void ItCanReceive()
        {
            Assert.IsTrue(m_SUT.Receive(1));
        }

        [Test]
        public void ItCanReduce()
        {
            Assert.IsTrue(m_SUT.Reduce(1));
        }

        [Test]
        public void ItReportsAmountLeftAfterReceiving()
        {
            m_SUT.Receive(10, out var leftovers);

            Assert.AreEqual(5, leftovers);
        }

        [Test]
        public void ItReportsAmountLeftAfterReducing()
        {
            m_SUT.Reduce(10, out var leftovers);

            Assert.AreEqual(5, leftovers);
        }

        [Test]
        public void ItDoesNotGoOverMaximum()
        {
            m_SUT.Receive(100);

            Assert.AreEqual(m_SUT.Max, m_SUT.Current);
        }

        [Test]
        public void ItDoesNotGoBelowZero()
        {
            m_SUT.Reduce(100);

            Assert.AreEqual(0, m_SUT.Current);
        }
    }

    [TestFixture]
    public class WhenFull : TestData
    {
        [SetUp]
        public void Setup()
        {
            m_currentAmount = m_Max;

            Initialize();
        }

        [Test]
        public void ItIsFull()
        {
            Assert.IsTrue(m_SUT.IsFull);
        }

        [Test]
        public void ItCantReceive()
        {
            Assert.IsFalse(m_SUT.Receive(1));
        }

        [Test]
        public void ItDoesntConsumeAmountReceived()
        {
            var receivingAmount = 10;

            m_SUT.Receive(receivingAmount, out int leftovers);

            Assert.AreEqual(receivingAmount, leftovers);
        }
    }

    [TestFixture]
    public class WhenEmpty : TestData
    {
        [SetUp]
        public void Setup()
        {
            m_currentAmount = 0;

            Initialize();
        }

        [Test]
        public void ItIsEmpty()
        {
            Assert.IsTrue(m_SUT.IsEmpty);
        }

        [Test]
        public void ItCantBeReduced()
        {
            Assert.IsFalse(m_SUT.Reduce(1));
        }

        [Test]
        public void ItDoesntConsumeAmountReduced()
        {
            var reducingAmount = 10;

            m_SUT.Reduce(reducingAmount, out int leftovers);

            Assert.AreEqual(reducingAmount, leftovers);
        }
    }
}