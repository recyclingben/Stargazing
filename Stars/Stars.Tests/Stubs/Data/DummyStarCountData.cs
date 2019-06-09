using System;
using System.Collections.Generic;
using System.Text;
using Stars.Data;
using System.Threading.Tasks;

namespace Stars.Tests.Stubs.Data
{
    class DummyStarCountData : IStarCountData
    {
        private long count = 0;

        public Task<long> GetCount() => Task.FromResult(count);

        public Task Increment(int amount)
        {
            count += amount;
            return Task.CompletedTask;
        }

        public void SetToRandom()
        {
            count = new Random().Next() % 10000;
        }
    }
}
