using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Timers;

namespace Stars.StarManagement
{
    public class StarBag
    {
        public readonly int EMPTY_FREQUENCY = 15 /* seconds */;
        public readonly object emptyLock = new object();
        public event Action<int> OnBagEmpty;

        private int StarCount { get; set; }


        public StarBag(Action<int> onBagEmpty)
        {
            OnBagEmpty += onBagEmpty;
            InitializeInterval();
        }

        public void Increment(int amount = 1)
        {
            if (amount < 1)
            {
                throw new ArgumentException($"Expected `amount` to be greater than zero, but was {amount}");
            }

            lock (emptyLock)
            {
                StarCount += amount;
            }
        }

        private void InitializeInterval()
        {
            var loop = new Timer()
            {
                Interval = EMPTY_FREQUENCY * 1000,
                AutoReset = true
            };
            loop.Elapsed += Flush;
            loop.Start();
        }

        private void Flush(object state, ElapsedEventArgs e)
        {
            lock (emptyLock)
            {
                if (!(OnBagEmpty is null))
                    OnBagEmpty(StarCount);
                StarCount = 0;
            }
        }
    }
}
