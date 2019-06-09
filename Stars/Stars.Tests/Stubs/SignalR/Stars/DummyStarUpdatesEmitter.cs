using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stars.SignalR.Stars;

namespace Stars.Tests.Stubs.SignalR.Stars
{
    class DummyStarUpdatesEmitter : IStarUpdatesEmitter
    {
        public int StarIncrementationEventsEmitted { get; set; } = 0;
        public Action<int> OnStarIncrementationEventEmitted;

        public Task EmitStarIncrementation(int amount)
        {
            ++StarIncrementationEventsEmitted;
            if (!(OnStarIncrementationEventEmitted is null))
                OnStarIncrementationEventEmitted(amount);
            return Task.CompletedTask;
        }
    }
}
