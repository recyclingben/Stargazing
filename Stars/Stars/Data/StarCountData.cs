using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stars.Data.Database;
using Microsoft.Extensions.Configuration;
using Stars.Extensions;

namespace Stars.Data
{
    public class StarCountData : IStarCountData
    {
        private readonly IConfiguration _config;

        public StarCountData(IConfiguration config)
        {
            _config = config;
        }

        public Task<long> GetCount()
        {
            using (var context = new StarContext(_config))
            {
                return Task.FromResult(context.StarCounts.Only().Count);
            }
        }

        public async Task Increment(int amount)
        {
            using (var context = new StarContext(_config))
            {
                await context.TransactionRetryOnFailure(async transaction => 
                {
                    /* Pessimistic lock. */
                    context.Database.ExecuteSqlCommand(
                        "SELECT null as dummy FROM dbo.starCounts WITH (tablockx, updlock)"
                    );
                    var starCount = context.StarCounts.Only();
                    starCount.Count = starCount.Count + amount;

                    await context.SaveChangesAsync();
                    transaction.Commit();
                }, retries: 3);
            }
        }
    }
}
