using ChenYiFan.ElasticSearch.Request;
using ChenYiFan.ElasticSearch.Tools.QueryGenerates;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChenYiFan.ElasticSearch.Params.MessageResponse;
using ChenYiFan.ElasticSearch.Tools.QueryExpressions;
using ElasticSearchUseDemo.Entities;
using ElasticSearchUseDemo.Extensions;
using System.Security.Cryptography;
using static ElasticSearchUseDemo.Enums.ProductEnum;

namespace ElasticSearchUseDemo.Controllers
{
    public class TextController : Controller
    {
        private readonly IRequestElasticSearch _requestElasticSearch;
        public TextController(IRequestElasticSearch requestElasticSearch)
        {
            _requestElasticSearch = requestElasticSearch;
        }

        [HttpPost("/Text/Product")]
        public async Task ProductionAsync()
        {
            var companyArray = Enum.GetValues(typeof(Company)) as Company[];
            var productTypeArray = Enum.GetValues(typeof(ProductType)) as ProductType[];
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Company = RandomGeneratorExtension.RandomArray(companyArray).ToString(),
                ProductType = RandomGeneratorExtension.RandomArray(productTypeArray).ToString(),
                Description = RandomGeneratorExtension.GenerateChineseWord(50),
                Remark = RandomGeneratorExtension.GenerateChineseWord(10),
                Price = Convert.ToDecimal(RandomNumberGenerator.Create().GeneratorDigitalRandom(3)),
                CreateTime = DateTime.Now
            };
            await _requestElasticSearch.IndexAsync<Product,Guid>(product);
        }

        [HttpPost("/Search/Expr")]
        public async Task<EsMessage<Product>> SearchExprAsync()
        {
            var queryNode = new QueryNode();
            queryNode.Where<Product>(x => x.Price < 10)
                .Where<Product>(x => x.Remark == "蜜")
                .From(0).Size(100);
            var res = await _requestElasticSearch.SearchAsync<Product, Guid>(queryNode);

            Console.WriteLine(res);

            return res;
        }

        [HttpPost("/Search/WhereIf")]
        public async Task<EsMessage<Product>> SearchWhereIfAsync(int? price, string remark)
        {
            var queryNode = new QueryNode();

            queryNode
                .WhereIf<Product>(price != null, x => x.Price < 10)
                .WhereIf<Product>(remark != null, x => x.Remark == remark)
                .From(0).Size(100);

            var res = await _requestElasticSearch.SearchAsync<Product, Guid>(queryNode);

            Console.WriteLine(res);

            return res;
        }
    }
}
