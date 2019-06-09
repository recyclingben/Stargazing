using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Stars.Data;
using Stars.StarManagement;
using Stars.Tests.Stubs.SignalR.Stars;
using Stars.Tests.Stubs.StarManagement;
using Stars.Tests.Stubs.Data;
using System.Threading.Tasks;

namespace Stars.Tests.Tests.StarManagement
{
    class Test_StarsTracker
    {
        public DummyStarUpdatesEmitter UpdatesEmitter { get; set; }
        public DummyStarCountData CountData { get; set; }
        public StarsTracker Tracker { get; set; }

        [SetUp]
        public void SetUp()
        {
            UpdatesEmitter = new DummyStarUpdatesEmitter();
            CountData = new DummyStarCountData();
            CountData.SetToRandom();
            Tracker = new StarsTracker(UpdatesEmitter, CountData);
        }



        [TestCase]
        public void Increment_ThrowsArgumentException_WhenCalledWithValueLessThanOne()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await Tracker.Increment(-1));
        }

        [MaxTime(StarBag.EMPTY_FREQUENCY * 1000 + 1000)]
        [TestCase(1)]
        [TestCase(10000)]
        public async Task Increment_EmitsStarIncrementation_WithExpectedValue(int incrementationAmount)
        {
            var promise = new TaskCompletionSource<int>();
            lock (Tracker.emptyLock)
            {
                Tracker.Increment(incrementationAmount);
                UpdatesEmitter.OnStarIncrementationEventEmitted += amount => promise.SetResult(amount);
            }
            Assert.AreEqual(incrementationAmount, await promise.Task);
        }

        [MaxTime(StarBag.EMPTY_FREQUENCY * 1000 + 1000)]
        [TestCase(1)]
        [TestCase(10000)]
        public async Task Increment_IncrementsData_ByExpectedValue(int incrementationAmount)
        {
            var beforeCount = await CountData.GetCount();

            var promise = new TaskCompletionSource<int>();
            lock (Tracker.emptyLock)
            {
                Tracker.Increment(incrementationAmount);
                UpdatesEmitter.OnStarIncrementationEventEmitted += amount => promise.SetResult(amount);
            }
            await promise.Task;

            Assert.AreEqual(beforeCount+incrementationAmount, await CountData.GetCount());
        }

        [TestCase]
        public async Task GetCount_ReturnsAmount_OfCorrectValue()
        {
            var count = await Tracker.GetCount();
            Assert.AreEqual(await CountData.GetCount(), count);
        }
    }
}
