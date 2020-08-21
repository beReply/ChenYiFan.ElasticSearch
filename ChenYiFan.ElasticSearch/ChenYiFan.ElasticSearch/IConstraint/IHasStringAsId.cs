using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasStringAsId : IHasId<string>
    {
        public new string Id { get; set; }
    }
}
