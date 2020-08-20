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

namespace ElasticSearchUseDemo.Controllers
{
    public class TextController : Controller
    {
        private readonly IRequestElasticSearch _requestElasticSearch;
        public TextController(IRequestElasticSearch requestElasticSearch)
        {
            _requestElasticSearch = requestElasticSearch;
        }

        [HttpPost("/Search/Expr")]
        public async Task<EsMessage<Product>> SearchExprAsync()
        {
            var queryNode = new QueryNode();
            queryNode.Where<Product>(x => x.Price < 10)
                .Where<Product>(x => x.Remark == "蜜")
                .From(0).Size(100);
            var res = await _requestElasticSearch.SearchAsync<Product>(queryNode);

            Console.WriteLine(res);

            return res;
        }
    }
}
