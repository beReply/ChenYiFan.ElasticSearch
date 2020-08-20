using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasGuidAsId
    {
        public Guid Id { get; set; }
    }
}
