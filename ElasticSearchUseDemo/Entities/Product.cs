using ChenYiFan.ElasticSearch.IConstraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchUseDemo.Entities
{
    public class Product : IHasGuidAsId
    {
        public Guid Id { get; set; }

        public string Company { get; set; }

        public string ProductType { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
