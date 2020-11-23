using Line.Messaging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
        private string AdminGroupId = Environment.GetEnvironmentVariable("AdminGroupID");
        private ILogger log;
        public Database(ILogger log)
        {
            this.log = log;
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            container = cosmosClient.GetContainer(databaseId, containerId);
            lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
        }
        public async Task UpdateMember()
        {
            var iterator = container.GetItemQueryIterator<Member>("SELECT * FROM c WHERE c.leavedDate = null");
            do
            {
                var result = await iterator.ReadNextAsync();
                foreach (var item in result)
                {
                    var user = lineMessagingClient.GetGroupMemberProfileAsync(GroupId, item.id);
                    var NewerName = user.Result.DisplayName;

                    if (NewerName != item.newername)
                    {
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
                            postScript = item.postScript,
                            leavedDate = item.leavedDate
                        };
                        await container.UpsertItemAsync(m);
                        try
                        {
                            await lineMessagingClient.PushMessageAsync(AdminGroupId, $"名前を変更しました\n入会時名前 {item.name}\n変更前名前 {item.newername}\n変更語名前 {NewerName}");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            } while (iterator.HasMoreResults);
        }
    }
}