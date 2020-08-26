using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasLongAsId : IHasId<long>
    {
        public new long Id { get; set; }
    }
}
