using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stars.StarManagement;
using Stars.Data;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Stars.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StarsController : ControllerBase
    {
        private readonly IStarsTracker _tracker;

        public StarsController(IStarsTracker tracker)
        {
            _tracker = tracker;
        }

        [HttpGet]
        [HttpGet("count")]
        [EnableCors("FullCors")]
        public async Task<ActionResult<JObject>> GET_Count()
        {
            var count = await _tracker.GetCount();
            var result = new
            {
                data = new { count }
            };

            return JObject.FromObject(result);
        }

        [HttpPost("increment")]
        [EnableCors("FullCors")]
        public async Task<ActionResult<JObject>> POST_Increment(int amount=1)
        {
            if (amount < 1)
            {
                Response.StatusCode = 422;
                var error = $"`amount` must be greater than or equal to 1, but was {amount}.";

                return JObject.FromObject(new { error });
            }

            Response.StatusCode = 200;
            await _tracker.Increment(amount);
            return new JObject();
        }
    }
}
