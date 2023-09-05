using Logger.Enums;
using Logger.Model;
using Nest;

namespace Logger.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient client;

        public ElasticsearchService(IElasticClient _client)
        {
            client = _client;
        }

        public void InsertActionLog(ActionLogModel actionModel)
        {
            string indexName = LogIndexType.action_log.ToString();
            if (!client.Indices.Exists(indexName).Exists)
            {
                client.Indices.Create(indexName, x => x.Map<ActionLogModel>(m => m.AutoMap()).Aliases(a => a.Alias(indexName)));
            }
            client.Index(actionModel, x => x.Index(indexName));
        }

        public void InsertErrorLog(ErrorLogModel errorModel)
        {
            string indexName = LogIndexType.error_log.ToString();
            if (!client.Indices.Exists(indexName).Exists)
            {
                client.Indices.Create(indexName, x => x.Map<ErrorLogModel>(m => m.AutoMap()).Aliases(a => a.Alias(indexName)));
            }
            client.Index(errorModel, x => x.Index(indexName));
        }

    }
}
