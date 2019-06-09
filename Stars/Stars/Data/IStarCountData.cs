using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stars.Data
{
    public interface IStarCountData
    {
        Task<long> GetCount();
        Task Increment(int amount);
    }
}
