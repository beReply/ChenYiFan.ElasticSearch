using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasDoubleAsId : IHasId<double>
    {
        public new double Id { get; set; }
    }
}
