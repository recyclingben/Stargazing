using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stars
{
    public static class DbSetExtensions
    {
        public static T Only<T>(this DbSet<T> self) where T : class
        {
            return self.First();
        }
    }
}
