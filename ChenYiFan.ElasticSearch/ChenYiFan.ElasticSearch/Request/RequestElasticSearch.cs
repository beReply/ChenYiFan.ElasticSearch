using ChenYiFan.ElasticSearch.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using ChenYiFan.ElasticSearch.Params;
using System.Threading.Tasks;
using ChenYiFan.ElasticSearch.Extensions;
using static ChenYiFan.ElasticSearch.Enums.ElasticSearchEnum;
using ChenYiFan.ElasticSearch.Tools.QueryGenerates;
using Newtonsoft.Json;
using ChenYiFan.ElasticSearch.IConstraint;
using ChenYiFan.ElasticSearch.Params.MessageResponse;
using Microsoft.Extensions.Options;

namespace ChenYiFan.ElasticSearch.Request
{
    public class RequestElasticSearch : IRequestElasticSearch
    {
        private readonly IOptionsMonitor<ElasticSearchConf> _elasticSearchConf;

        public RequestElasticSearch(IOptionsMonitor<ElasticSearchConf> elasticSearchConf)
        {
            _elasticSearchConf = elasticSearchConf;
        }

        #region CURD

        /// <summary>
        /// 索引一个文档
        ///     如果ID不存在，创建新的文档。否则先删除现在的文档，再创建新的文档，版本会增加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<EsResponseResult> IndexAsync<T, TId>(T data) where T : IHasId<TId>
        {
            return await SendAsync(HttpMethod.Put, typeof(T).Name.ToLower(),
                    ElasticOperation._doc.ToString(), data.Id.ToString(), JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 索引一个文档
        ///     如果ID已经存在，会失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<EsResponseResult> CreateAsync<T, TId>(T data) where T : IHasId<TId>
        {
            return await SendAsync(HttpMethod.Put, typeof(T).Name.ToLower(),
                    ElasticOperation._create.ToString(), data.Id.ToString(), JsonConvert.SerializeObject(data));
        }

        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EsResponseResult> DeleteAsync<T, TId>(Guid id) where T : IHasId<TId>
        {
            return await SendAsync(HttpMethod.Delete, typeof(T).Name.ToLower(),
                    ElasticOperation._doc.ToString(), id.ToString(), null);
        }

        /// <summary>
        /// 更新一个文档
        ///     文档必须已经存在，更新会对相应字段做增量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<EsResponseResult> UpdateAsync<T, TId>(T data) where T : IHasId<TId>
        {
            return await SendAsync(HttpMethod.Post, typeof(T).Name.ToLower(),
                    ElasticOperation._update.ToString(), data.Id.ToString(), JsonConvert.SerializeObject(new { doc = data }));
        }

        #endregion

        #region 查询

        public async Task<EsMessage<T>> SearchAsync<T, TId>(QueryNode queryNode) where T : IHasId<TId>
        {
            var body = queryNode.GenerateQueryString();

            var res = await SendAsync(HttpMethod.Get, typeof(T).Name.ToLower(),
                    ElasticOperation._search.ToString(), null, body);

            if (res.IsSuccess)
            {
                return JsonConvert.DeserializeObject<EsMessage<T>>(res.Message);
            }

            return new EsMessage<T>();
        }

        #endregion


        #region 私有工具

        private async Task<EsResponseResult> SendAsync(HttpMethod httpMethod, string table, string operation, string handle, string body)
        {
            if (table.IsNullOrWhiteSpace() || operation.IsNullOrWhiteSpace())
            {
                Console.WriteLine("参数错误");
                return new EsResponseResult { IsSuccess = false, Message = "参数错误" };
            }

            var path = $"{_elasticSearchConf.CurrentValue.Url}/" + $"{table}/{operation}/{handle}".Trim('/').Replace("//", "/");
            Console.WriteLine(path);

            EsResponseResult esHttpResult;
            try
            {
                var client = new HttpClient();
                var content = new StringContent(body, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var request = new HttpRequestMessage(httpMethod, path) { Content = content };
                var response = await client.SendAsync(request);

                client.Dispose();
                esHttpResult = new EsResponseResult { IsSuccess = true, Message = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception e)
            {
                Console.WriteLine("请求失败");
                Console.WriteLine(e);
                return new EsResponseResult { IsSuccess = false, Message = "请求失败" };
            }

            return esHttpResult;
        }

        #endregion
    }
}
