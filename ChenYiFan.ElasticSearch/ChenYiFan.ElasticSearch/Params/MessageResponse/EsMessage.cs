using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Params.MessageResponse
{
    public class EsMessage<T>
    {
        public int took { get; set; }

        public bool timed_out { get; set; }

        public Shards _shards { get; set; }

        public Hits<T> hits { get; set; }
    }
}
