using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    public class Database
    {
        private readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
        private readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
        private CosmosClient cosmosClient;
        private Container container;
        private string databaseId = Environment.GetEnvironmentVariable("CosmosDBId");
        private string containerId = Environment.GetEnvironmentVariable("CosmosDBContainerId");
        public Database()
        {
            cosmosClient = new CosmosClient(this.EndpointUri, this.PrimaryKey);
            container = cosmosClient.GetContainer(databaseId, containerId);
        }
        public async Task<bool> MemberCheck(string Id)
        {
            var responce = await container.ReadItemAsync<Member>(Id, new PartitionKey("freedom"));
            if (responce.Resource.check == "済")
            {
                return false;
            }
            else
            {
                var m = new Member
                {
                    id = Id,
                    name = responce.Resource.name,
                    joinedDate = responce.Resource.joinedDate,
                    check = "済",
                    postScript = responce.Resource.postScript
                };
                await container.UpsertItemAsync(m);
                return true;
            }
        }
        public async Task MemberDelete(string ID)
        {
            var m = new Member
            {
                id = ID
            };
            await container.DeleteItemAsync<Member>(m.id, new PartitionKey(m.group));
        }
        public async Task MemberAdd(string Id, string Name, string JoinedDate, string Check = null, string PostScript = null)
        {
            var m = new Member
            {
                id = Id,
                name = Name,
                joinedDate = JoinedDate,
                check = Check,
                postScript = PostScript
            };

            await container.CreateItemAsync(m);
        }
        public async Task MemberCheckReset()
        {
            var iterator = container.GetItemQueryIterator<Member>("SELECT * FROM c Where c.check != null");
            do
            {
                var result = await iterator.ReadNextAsync();

                foreach (var item in result)
                {
                    var m = new Member
                    {
                        id = item.id,
                        name = item.name,
                        joinedDate = item.joinedDate,
                        check = null,
                        postScript = item.postScript
                    };
                    await container.UpsertItemAsync(m);
                }
            } while (iterator.HasMoreResults);
        }

        public class Member
        {
            public string id { get; set; }
            public string group = "freedom";
            public string name { get; set; }
            public string joinedDate { get; set; }
            public string check { get; set; }
            public string postScript { get; set; }
        }
    }
}
