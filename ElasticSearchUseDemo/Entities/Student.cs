using ChenYiFan.ElasticSearch.IConstraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchUseDemo.Entities
{
    public class Student : IHasIntAsId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public DateTime EnrollmentTime { get; set; }

        public string Remark { get; set; }
    }
}
