using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Models
{
    [Serializable]
    public class ResultModel<T>
    {
        public T Data { get; set; }
        public T Error { get; set; }
    }
}
