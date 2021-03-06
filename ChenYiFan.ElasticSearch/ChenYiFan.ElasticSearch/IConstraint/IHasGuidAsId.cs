﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.IConstraint
{
    public interface IHasGuidAsId : IHasId<Guid>
    {
        public new Guid Id { get; set; }
    }
}
