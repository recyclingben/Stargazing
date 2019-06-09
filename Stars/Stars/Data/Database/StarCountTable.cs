using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stars.Data.Database
{
    public class StarCountTable : DbContext
    {
        public byte Id { get; set; }
        public long Count { get; set; }
    }
}
