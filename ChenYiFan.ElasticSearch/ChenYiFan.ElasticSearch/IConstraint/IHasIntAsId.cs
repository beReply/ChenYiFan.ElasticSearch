using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasIntAsId : IHasId<int>
    {
        public new int Id { get; set; }
    }
}
