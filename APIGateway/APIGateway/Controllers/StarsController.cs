using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APIGateway.ServiceUtility.ClusterRequests;
using APIGateway.ServiceUtility.StarUpdatesDataManagement;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using APIGateway.Models;

namespace APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public partial class StarsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IClusterRequestAgent _clusterRequestAgent;
        private readonly StarIncrementationAgreggator _starUpdatesDataAggregator;

        public StarsController(IConfiguration config, IClusterRequestAgent clusterRequestAgent, StarIncrementationAgreggator starUpdatesDataAggregator)
        {
            _config = config;
            _clusterRequestAgent = clusterRequestAgent;
            _starUpdatesDataAggregator = starUpdatesDataAggregator;
        }


        [HttpGet("count")]
        [HttpGet]
        [EnableCors("FullCors")]
        public async Task<ActionResult<ResultModel<StarCount>>> GET_Count(int id)
        {
            return await GetStarCountJsonAsync();
        }


        [HttpGet("est-frequency")]
        [EnableCors("FullCors")]
        public ActionResult<ResultModel<StarRecentFrequency>> GET_RecentIncrementationFrequency(int id)
        {
            return GetRecentIncrementationFrequency();
        }


        [HttpPost("increment")]
        [EnableCors("FullCors")]
        public async Task<ActionResult> POST_Increment(int id)
        {
            await IncrementStarCountAsync();
            return new EmptyResult();
        }
    }



    public partial class StarsController {
        private async Task IncrementStarCountAsync()
        {
            var requestOptions = new ClusterRequestOptions(_config.ServiceHost("stars"), "/stars/increment", HttpMethod.Post);
            await _clusterRequestAgent.RequestNoContent(requestOptions);
        }


        private async Task<ResultModel<StarCount>> GetStarCountJsonAsync()
        {
            var requestOptions = new ClusterRequestOptions(_config.ServiceHost("stars"), "/stars", HttpMethod.Get);
            var response = await _clusterRequestAgent.Request<ResultModel<StarCount>>(requestOptions);
            return response;
        }


        private ResultModel<StarRecentFrequency> GetRecentIncrementationFrequency()
        {
            var frequency = _starUpdatesDataAggregator.Frequency;

            return new ResultModel<StarRecentFrequency>
            {
                Data = new StarRecentFrequency
                {
                    Frequency = frequency
                }
            };
        }
    }
}
