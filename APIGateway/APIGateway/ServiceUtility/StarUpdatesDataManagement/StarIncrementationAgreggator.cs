using APIGateway.SignalR.Stars.StarUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.ServiceUtility.StarUpdatesDataManagement
{
    /* Holds need data in received SignalR updates to be held for use later. */
    public class StarIncrementationAgreggator
    {
        public int Frequency { get; set; } = 0;

        public StarIncrementationAgreggator(StarUpdatesListener listener)
        {
            listener.OnStarIncrementationEventReceived += HandleStarIncrementation;
        }

        /* Add amount to `RecentIncrementationAccumulation` and remove it in 15 seconds. */
        private void HandleStarIncrementation(int amount)
        {
            Frequency += amount;

            Task.Delay(15000).ContinueWith(t =>
            {
                Frequency -= amount;
            });
        }
    }
}
