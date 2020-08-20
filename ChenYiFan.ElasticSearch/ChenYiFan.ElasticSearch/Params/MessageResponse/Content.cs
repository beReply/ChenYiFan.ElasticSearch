using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Params.MessageResponse
{
    public class Content<T>
    {
        public string _index { get; set; }

        public string _type { get; set; }

        public string _id { get; set; }

        public string _score { get; set; }

        public T _source { get; set; }
    }
}
