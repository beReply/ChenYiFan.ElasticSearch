using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Params.MessageResponse
{
    public class Shards
    {
        public int total { get; set; }

        public int successful { get; set; }

        public int skipped { get; set; }

        public int failed { get; set; }
    }
}
