using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchUseDemo.Enums
{
    public class ProductEnum
    {
        public enum Company
        {
            幻想乡影业 = 1,
            神医制药厂 = 2,
            雷电食品加工厂 = 3,
            奥托帽子厂 = 4
        }

        public enum ProductType
        {
            娱乐 = 1,
            药品 = 2,
            食品 = 3,
            服饰 = 4

        }
    }
}
