using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.SignalR.APIGateway.StarUpdates.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace APIGateway.SignalR.APIGateway.StarUpdates
{
    public class StarUpdatesEmitter
    {
        private readonly IHubContext<StarUpdatesHub> _hubContext;
        public StarUpdatesEmitter(IConfiguration config, IHubContext<StarUpdatesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task EmitStarIncrementationAsync(int amount)
        {
            var data = new StarIncrementationEventData
            {
                Amount = amount
            };
            await _hubContext.Clients.All.SendAsync("StarIncremented", data);
        }
    }
}
