using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Stars.StarManagement;

namespace Stars.Tests.Tests.StarManagement
{
    class Test_StarBag
    {
        public StarBag Bag { get; set; }

        [SetUp]
        public void SetUp()
        {
            /* 
             * Initialize the bag with dummy event handler. 
             * Tests should manually issue their own event handlers with `OnBagFlush`. 
             */
            Bag = new StarBag(dummy => { });
        }

        [TestCase]
        public void Increment_ThrowsArgumentException_WhenCalledWithNumberBelowOne()
        {
            Assert.Throws<ArgumentException>(() => Bag.Increment(-1));
        }

        [MaxTime(StarBag.EMPTY_FREQUENCY * 1000 + 1000)]
        [TestCase(2000)]
        public async Task Increment_TriggersOnBagFlush_WithExpectedValue(int incrementationAmount)
        {
            var promise = new TaskCompletionSource<int>();
            lock (Bag.emptyLock)
            {
                Bag.Increment(incrementationAmount);
                Bag.OnBagEmpty += amount => promise.SetResult(amount);
            }
            Assert.AreEqual(incrementationAmount, await promise.Task);
        }
    }
}
