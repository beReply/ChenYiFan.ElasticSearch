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
        Task<EsResponseResult> IndexAsync<T>(T data) where T : IHasGuidAsId;

        Task<EsResponseResult> CreateAsync<T>(T data) where T : IHasGuidAsId;

        Task<EsResponseResult> DeleteAsync<T>(Guid id) where T : IHasGuidAsId;

        Task<EsResponseResult> UpdateAsync<T>(T data) where T : IHasGuidAsId;

        Task<EsMessage<T>> SearchAsync<T>(QueryNode queryNode) where T : IHasGuidAsId;
    }
}
