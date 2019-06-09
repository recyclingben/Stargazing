using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stars.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task TransactionRetryOnFailure(this DbContext context, Func<IDbContextTransaction, Task> fn, int retries = 3)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                for (int i = 0; i < retries; ++i)
                {
                    try
                    {
                        await fn(transaction);
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"ERROR: {e}");
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
