using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Stars.Data;
using Stars.Extensions;
using Stars.SignalR.Stars;
using Newtonsoft.Json.Linq;
using Data = Stars.Data;

namespace Stars.StarManagement
{
    public class StarsTracker : IStarsTracker
    {
        private readonly StarBag _starBag;
        private readonly IStarUpdatesEmitter _updateEmitter;
        private readonly IStarCountData _starCount;
        public readonly object emptyLock;


        public StarsTracker(IStarUpdatesEmitter updateEmitter, IStarCountData starCount)
        {
            _updateEmitter = updateEmitter;
            _starBag = new StarBag(async amount =>
            {
                await IncrementStars(amount);
            });
            emptyLock = _starBag.emptyLock;
            _starCount = starCount;
        }

        public async Task<long> GetCount() => await _starCount.GetCount();

        public Task Increment(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentException($"Expected `amount` to be greater than 0, but was {amount}");
            }

            SoftIncrement(amount);
            return Task.CompletedTask;
        }


        /* Add to the bag of stars. Don't update to database. */
        private void SoftIncrement(int amount = 1)
        {
            _starBag.Increment(amount);
        }

        /* Update database with these values. Should be called when bag of stars is emptied*/
        private async Task HardIncrement(int amount = 1)
        {
            if (amount <= 0) return;

            await _starCount.Increment(amount);
        }

        private async Task IncrementStars(int amount)
        {
            if (amount <= 0) return;

            await Task.WhenAll
            (
                HardIncrement(amount),
                _updateEmitter.EmitStarIncrementation(amount)
            );
        }
    }
}
