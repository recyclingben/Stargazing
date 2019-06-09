using System;
using System.Threading.Tasks;
using Stars.StarManagement;
using Stars.Tests.Stubs.Data;

namespace Stars.Tests.Stubs.StarManagement
{
    public class DummyStarsTracker : IStarsTracker
    {
        private readonly DummyStarCountData data;

        public Task<long> GetCount() => data.GetCount();


        public DummyStarsTracker()
        {
            data = new DummyStarCountData();
        }

        public Task Increment(int amount = 1)
        {
            return data.Increment(amount);
        }

        public Task SetToRandom()
        {
            data.SetToRandom();
            return Task.CompletedTask;
        }
    }
}
