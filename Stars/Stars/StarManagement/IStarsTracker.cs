using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Stars.StarManagement
{
    public interface IStarsTracker
    {
        Task<long> GetCount();
        Task Increment(int amount=1);
    }
}
