# ChenYiFan.ElasticSearch

<p>一、配置</p>
<p>     在appsettings.json中添加（默认）</p>
<p>         "ElasticSearch": {</p>
<p>             "Url": "http://localhost:9200" // ElasticSearch的路径</p>
<p>}        </p>
<p>     在Startup中添加</p>
<p>         services.AddCyfElasticSearchConf(_configuration);</p>
   
<p>     或者使用如下的方式自定义字段（自定义）</p>
<p>         appsettings.json:</p>
<p>             "beReplyElasticSearch": {</p>
<p>                 "Url": "http://localhost:9200" // ElasticSearch的路径</p>
<p>             }</p>
<p>         Startup.cs</p>
<p>             services.AddCyfElasticSearchConf(_configuration, "beReplyElasticSearch");</p>
   

  
  
