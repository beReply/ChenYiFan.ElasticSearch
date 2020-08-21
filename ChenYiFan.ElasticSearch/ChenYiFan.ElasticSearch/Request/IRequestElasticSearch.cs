using ChenYiFan.ElasticSearch.IConstraint;
using ChenYiFan.ElasticSearch.Params;
using ChenYiFan.ElasticSearch.Params.MessageResponse;
using ChenYiFan.ElasticSearch.Tools.QueryGenerates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChenYiFan.ElasticSearch.Request
{
    public interface IRequestElasticSearch
    {
        Task<EsResponseResult> IndexAsync<T, TId>(T data) where T : IHasId<TId>;

        Task<EsResponseResult> CreateAsync<T, TId>(T data) where T : IHasId<TId>;

        Task<EsResponseResult> DeleteAsync<T, TId>(Guid id) where T : IHasId<TId>;

        Task<EsResponseResult> UpdateAsync<T, TId>(T data) where T : IHasId<TId>;

        Task<EsMessage<T>> SearchAsync<T, TId>(QueryNode queryNode) where T : IHasId<TId>;
    }
}
