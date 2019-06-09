using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.SignalR;
using Stars.SignalR.Stars.Data;

namespace Stars.SignalR.Stars
{
    public class StarUpdatesEmitter : IStarUpdatesEmitter
    {
        private readonly IHubContext<StarUpdatesHub> _hubContext;
        public StarUpdatesEmitter(IConfiguration config, IHubContext<StarUpdatesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task EmitStarIncrementation(int amount)
        {
            var data = new StarIncrementationEventData
            {
                Amount = amount
            };
            await _hubContext.Clients.All.SendAsync("StarIncremented", data);
        }
    }
}
