using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using APIGateway.SignalR.Stars.StarUpdates.Data;

namespace APIGateway.SignalR.Stars.StarUpdates
{
    public class StarUpdatesListener
    {
        public event Action<int> OnStarIncrementationEventReceived;

        private readonly HubConnection _hubConnection;

        public StarUpdatesListener(IConfiguration config)
        {
            string starsServiceIP = config.ServiceHost("stars");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{starsServiceIP}/signalr/starupdates")
                .Build();

            SetUpEventReceivers();
        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        private void SetUpEventReceivers()
        {
            _hubConnection.On<StarIncrementationEventData>("StarIncremented", data => 
            {
                OnStarIncrementationEventReceived?.Invoke(data.Amount);
            });
        }
    }
}
