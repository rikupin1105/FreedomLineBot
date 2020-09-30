using Microsoft.Azure.Cosmos;
using System;
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
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
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
                    newername = responce.Resource.newername,
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
            await container.DeleteItemAsync<Member>(ID, new PartitionKey("freedom"));
        }
        public async Task MemberAdd(Member m)
        {
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
                        newername = item.newername,
                        joinedDate = item.joinedDate,
                        check = null,
                        postScript = item.postScript
                    };
                    await container.UpsertItemAsync(m);
                }
            } while (iterator.HasMoreResults);
        }
        public async Task GetMember(string query)
        {
            var iterator = container.GetItemQueryIterator<Member>(query);
            var sMember = "";
            do
            {
                var result = await iterator.ReadNextAsync();

                foreach (var item in result)
                {
                    sMember += "\n" + item.newername;
                }
            } while (iterator.HasMoreResults);
            Sentence = sMember;
        }
        public static string Sentence { get; set; }
        
    }
}