using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway.ServiceUtility.ClusterRequests
{
    public class ClusterRequestOptions
    {
        public string DeploymentIp { get; }
        public string Path { get; }
        public HttpMethod Method { get; }

        public ClusterRequestOptions(string deploymentIp, string path, HttpMethod method)
        {
            DeploymentIp = deploymentIp;
            Path = path;
            Method = method;
        }
    }
}
