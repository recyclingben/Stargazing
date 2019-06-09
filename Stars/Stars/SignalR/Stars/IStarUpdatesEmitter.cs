using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stars.SignalR.Stars
{
    public interface IStarUpdatesEmitter
    {
         Task EmitStarIncrementation(int amount);
    }
}
