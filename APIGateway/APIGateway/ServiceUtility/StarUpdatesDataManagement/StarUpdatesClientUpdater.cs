using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.SignalR.Stars.StarUpdates;
using APIGateway.SignalR.APIGateway.StarUpdates;


namespace APIGateway.ServiceUtility.StarUpdatesDataManagement
{
    public class StarUpdatesClientUpdater
    {
        private readonly StarUpdatesEmitter _updateEmitter;
        public StarUpdatesClientUpdater(StarUpdatesListener updateListener, StarUpdatesEmitter updateEmitter)
        {
            _updateEmitter = updateEmitter;
            updateListener.OnStarIncrementationEventReceived += HandleStarIncrementationEventAsync;

        }

        /* This method is async with void return type because it is an event handler. */
        private async void HandleStarIncrementationEventAsync(int amount)
        {
            await _updateEmitter.EmitStarIncrementationAsync(amount);
        }
    }
}
