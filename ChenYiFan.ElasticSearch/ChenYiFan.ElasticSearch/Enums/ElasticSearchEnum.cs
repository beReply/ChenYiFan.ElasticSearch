using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Enums
{
    public class ElasticSearchEnum
    {
        public enum ElasticOperation
        {
            _doc = 1,
            _create = 2,
            _update = 3,
            _search = 4
        }

        public enum NodeType
        {
            普通节点 = 0,
            叶子节点 = 1,
            数组节点 = 2
        }
    }
}
