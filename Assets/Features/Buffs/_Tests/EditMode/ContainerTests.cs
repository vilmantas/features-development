using System.Linq;
using Features.Buffs;
using NUnit.Framework;

namespace BuffContainerTests
{
    public class TestData
    {
        protected const int MaxStackLimit = 3;
        protected readonly BuffBase Simple = new("Simple", 5f);

        protected readonly BuffBase Stackable = new("Stackable", 5f, MaxStackLimit);
        protected BuffContainer _sut;

        protected ActiveBuff LastRemoved, LastStackRemoved, LastStackAdded, LastAdded, LastReset;

        protected int tickCalls, addCalls, removeCalls, stackAddCalls, stackRemoveCalls, resetCallbacks;

        protected BuffBase WithImmediateIntervalExecution;

        protected BuffBase WithInterval;

        protected BuffBase WithMinimumInterval;


        public void TickCallback(ActiveBuff buff)
        {
            tickCalls++;
        }

        public void OnAdded(ActiveBuff buff)
        {
            LastAdded = buff;
            addCalls++;
        }

        public void OnRemoved(ActiveBuff buff)
        {
            LastRemoved = buff;
            removeCalls++;
        }

        public void OnStackAdded(ActiveBuff buff)
        {
            LastStackAdded = buff;
            stackAddCalls++;
        }

        public void OnStackRemoved(ActiveBuff buff)
        {
            LastStackRemoved = buff;
            stackRemoveCalls++;
        }

        public void OnDurationReset(ActiveBuff buff)
        {
            resetCallbacks++;
            LastReset = buff;
        }
    }

    public class General : TestData
    {
        [SetUp]
        public void SetUp()
        {
            _sut = new BuffContainer().RegisterCallbacks(OnRemoved, OnAdded, OnStackRemoved, OnStackAdded,
                TickCallback, OnDurationReset);

            WithInterval = new BuffBase("Interval", 5f).WithInterval(1f);

            WithMinimumInterval = new BuffBase("IntervalMin", 5f).WithInterval(0.1f);

            WithImmediateIntervalExecution = new BuffBase("IntervalMin", 5f).WithInterval(100f, true);

            stackAddCalls = stackRemoveCalls = removeCalls = addCalls = tickCalls = 0;
        }

        [Test]
        public void ItCanReceiveABuff()
        {
            _sut.Receive(Simple);

            Assert.AreEqual(1, _sut.Buffs.Count);
        }

        [Test]
        public void ItAddsOneStackbyDefault()
        {
            _sut.Receive(Simple);

            Assert.AreEqual(1, _sut.Buffs.First().Stacks);
        }

        [Test]
        public void ItStacksBuffs()
        {
            _sut.Receive(Stackable);
            _sut.Receive(Stackable);

            Assert.AreEqual(2, _sut.Buffs.First().Stacks);
        }

        [Test]
        public void ItDoesNotStackOverLimit()
        {
            _sut.Receive(Stackable, MaxStackLimit + 10);

            Assert.AreEqual(MaxStackLimit, _sut.Buffs.First().Stacks);
        }

        [Test]
        public void AddsABuffWithFullDuration()
        {
            _sut.Receive(Simple);

            Assert.AreEqual(Simple.Duration, _sut.Buffs.First().DurationLeft);
        }

        [Test]
        public void ItTriggersInterval()
        {
            _sut.Receive(WithInterval);

            _sut.Tick(WithInterval.TickInterval);

            Assert.AreEqual(1, tickCalls);
        }

        [Test]
        public void ItTriggersTickIntervalsDuringBuffDuration()
        {
            _sut.Receive(WithInterval);

            _sut.Tick(50f);

            Assert.AreEqual(5, tickCalls);
        }

        [Test]
        public void ItNotifiesWhenBuffReceived()
        {
            _sut.Receive(Simple);

            Assert.AreEqual(1, addCalls);
            Assert.AreSame(Simple, LastAdded.Metadata);
        }

        [Test]
        public void ItNotifiesWhenBuffRemoved()
        {
            _sut.Receive(Simple);

            _sut.Tick(50f);

            Assert.AreEqual(1, removeCalls);
            Assert.AreSame(Simple, LastRemoved.Metadata);
        }

        [Test]
        public void ItCanReceiveBuffAfterFirstFinishes()
        {
            _sut.Receive(Stackable);

            _sut.Tick(50f);

            _sut.Receive(Stackable);

            Assert.AreEqual(2, addCalls);
            Assert.AreEqual(1, removeCalls);
            Assert.AreSame(Stackable, LastRemoved.Metadata);
        }

        [Test]
        public void ItShouldTick10TimesForEverySecondPassed()
        {
            _sut.Receive(WithMinimumInterval);

            for (int i = 0; i < 5000; i++)
            {
                _sut.Tick(0.007326f);
            }

            Assert.AreEqual(WithMinimumInterval.Duration * 10, tickCalls);
        }

        [Test]
        public void ItNotifiesWhenStackWasAdded()
        {
            _sut.Receive(Stackable);

            _sut.Receive(Stackable);

            Assert.AreEqual(1, stackAddCalls);
            Assert.AreSame(Stackable, LastStackAdded.Metadata);
        }

        [Test]
        public void ItNotifiesWhenStackWasRemoved()
        {
            _sut.Receive(Stackable, 5);

            _sut.Tick(6f);

            Assert.AreEqual(1, stackRemoveCalls);
            Assert.AreSame(Stackable, LastStackRemoved.Metadata);
        }

        [Test]
        public void ItNotifiesEachTimeAStackGetsRemoved()
        {
            _sut.Receive(Stackable, Stackable.MaxStack);

            _sut.Tick(99f);

            Assert.AreEqual(Stackable.MaxStack, stackRemoveCalls);
            Assert.AreSame(Stackable, LastStackRemoved.Metadata);
        }

        [Test]
        public void ItNotifiesEachTimeWhenStacksGetAdded()
        {
            _sut.Receive(Stackable, Stackable.MaxStack);

            _sut.Receive(Stackable);

            _sut.Receive(Stackable);

            _sut.Receive(Stackable);

            Assert.AreEqual(3, stackAddCalls);
            Assert.AreSame(Stackable, LastStackAdded.Metadata);
        }

        [Test]
        public void ItCanExecuteIntervalTickImmediately()
        {
            _sut.Receive(WithImmediateIntervalExecution);

            _sut.Tick(0.1f);

            Assert.AreEqual(1, tickCalls);
        }
    }
}