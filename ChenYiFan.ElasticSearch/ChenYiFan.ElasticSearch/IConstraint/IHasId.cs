using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasId<TId>
    {
        public TId Id { get; set; }
    }
}
