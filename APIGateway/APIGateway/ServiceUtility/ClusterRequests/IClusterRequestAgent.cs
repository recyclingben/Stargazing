using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway.ServiceUtility.ClusterRequests
{
    public interface IClusterRequestAgent
    {
        Task RequestNoContent(ClusterRequestOptions options);
        Task<T> Request<T>(ClusterRequestOptions options);
    }
}
