using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using APIGateway.Models;

namespace APIGateway.ServiceUtility.ClusterRequests
{
    public class ClusterRequestAgent : IClusterRequestAgent
    {
        private HttpClient Client { get; }

        public ClusterRequestAgent()
        {
            Client = new HttpClient();
        }

        public async Task RequestNoContent(ClusterRequestOptions options)
        {
            var ip = options.DeploymentIp;
            var path = options.Path;
            var method = options.Method;

            var uri = new Uri($"http://{ip}{path}");

            var message = new HttpRequestMessage(method, uri);

            await Client.SendAsync(message);
        }

        public async Task<T> Request<T>(ClusterRequestOptions options)
        {
            var ip = options.DeploymentIp;
            var path = options.Path;
            var method = options.Method;

            var uri = new Uri($"http://{ip}{path}");

            var message = new HttpRequestMessage(method, uri);

            var response = await Client.SendAsync(message);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse);
        }
    }
}
