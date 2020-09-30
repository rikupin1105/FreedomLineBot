using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Line.Messaging;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;

namespace UpdateMemberName
{
    public class Database
    {
        private readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
        private readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
        private CosmosClient cosmosClient;
        private Container container;
        private string databaseId = Environment.GetEnvironmentVariable("CosmosDBId");
        private string containerId = Environment.GetEnvironmentVariable("CosmosDBContainerId");
        private LineMessagingClient lineMessagingClient;
        private string GroupId = Environment.GetEnvironmentVariable("GroupId");
        private ILogger log;
        public Database(ILogger log)
        {
            this.log = log;
            cosmosClient = new CosmosClient(this.EndpointUri, this.PrimaryKey);
            container = cosmosClient.GetContainer(databaseId, containerId);
            lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
        }
        public async Task UpdateMember()
        {
            var iterator = container.GetItemQueryIterator<Member>("SELECT * FROM c");
            do
            {
                var result = await iterator.ReadNextAsync();
                foreach (var item in result)
                {
                    var user = lineMessagingClient.GetGroupMemberProfileAsync(GroupId, item.id);
                    var NewerName = user.Result.DisplayName;
                    log.LogInformation(item.id);
                    log.LogInformation(item.name);
                    log.LogInformation(NewerName);
                    var m = new Member
                    {
                        id = item.id,
                        name = item.name,
                        newername = NewerName,
                        joinedDate = item.joinedDate,
                        check = item.check,
                        postScript = item.postScript
                    };
                    await container.UpsertItemAsync(m);
                }
            } while (iterator.HasMoreResults);
        }
    }
}