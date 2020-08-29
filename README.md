# ChenYiFan.ElasticSearch

一、配置
  在appsettings.json中添加
    "ElasticSearch": {
      "Url": "http://localhost:9200" // ElasticSearch的路径
    }
  在Startup中添加
   services.AddCyfElasticSearchConf(_configuration);
   
  同时也不一定需要使用ElasticSearch作为config的section名，可以使用如下的方式自定义字段
  appsettings.json:
    "beReplyElasticSearch": {
      "Url": "http://localhost:9200" // ElasticSearch的路径
    }
    Startup.cs
    services.AddCyfElasticSearchConf(_configuration, "beReplyElasticSearch");
   

  
  
