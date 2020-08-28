using ChenYiFan.ElasticSearch.Configuration;
using ChenYiFan.ElasticSearch.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddCyfElasticSearchConf(this IServiceCollection services, IConfiguration configuration, string key = "ElasticSearch")
        {
            services.Configure<ElasticSearchConf>(configuration.GetSection(key));
            services.AddTransient<IRequestElasticSearch, RequestElasticSearch>();
            return services;
        }
    }
}
