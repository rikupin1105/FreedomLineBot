using Line.Messaging;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    public class Database
    {
        private readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
        private readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
        private string databaseId = Environment.GetEnvironmentVariable("CosmosDBId");
        private string containerId = Environment.GetEnvironmentVariable("CosmosDBContainerId");
        private string GroupId = Environment.GetEnvironmentVariable("GroupId");
        private CosmosClient cosmosClient;
        private Container container;
        public Database()
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            container = cosmosClient.GetContainer(databaseId, containerId);
        }
        public async Task<bool> RejoinCheck(string Id)
        {
            var count = 0;
            var iterator = container.GetItemQueryIterator<Member>($"SELECT * FROM c WHERE c.id = \"{Id}\"");
            do
            {
                await iterator.ReadNextAsync();
                count++;
            } while (iterator.HasMoreResults);

            if (count == 0) return false;
            else return true;
        }
        public async Task<checkResult> MemberCheck(string Id)
        {
            var responce = await container.ReadItemAsync<Member>(Id, new PartitionKey("freedom"));
            if (responce.Resource.check != null )
            {
                return new checkResult { already = true };
            }
            else
            {
                await container.UpsertItemAsync(new Member
                {
                    id = Id,
                    name = responce.Resource.name,
                    newername = responce.Resource.newername,
                    joinedDate = responce.Resource.joinedDate,
                    check = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd HH:mm"),
                    postScript = responce.Resource.postScript
                });
                return new checkResult { already = false, name = responce.Resource.newername };
            }
        }
        public class checkResult
        {
            public bool already { get; set; }
            public string name { get; set; }
        }

        public async Task MemberLeave(string Id)
        {
            var responce = await container.ReadItemAsync<Member>(Id, new PartitionKey("freedom"));
            await container.UpsertItemAsync(new Member
            {
                id = Id,
                name = responce.Resource.name,
                newername = responce.Resource.newername,
                joinedDate = responce.Resource.joinedDate,
                check = responce.Resource.check,
                postScript = responce.Resource.postScript,
                leavedDate = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd HH:mm")
            });
        }
        public async Task MemberAdd(Member m)
        {
            await container.UpsertItemAsync(m);
        }
        public async Task MemberCheckReset()
        {
            var iterator = container.GetItemQueryIterator<Member>("SELECT * FROM c Where c.check != null and c.leavedDate = null");
            do
            {
                var result = await iterator.ReadNextAsync();

                foreach (var item in result)
                {
                    await container.UpsertItemAsync(new Member
                    {
                        id = item.id,
                        name = item.name,
                        newername = item.newername,
                        joinedDate = item.joinedDate,
                        check = null,
                        postScript = item.postScript
                    });
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
        public async Task GetFormerMember(string query)
        {
            var iterator = container.GetItemQueryIterator<Member>(query);
            var sMember = "";
            do
            {
                var result = await iterator.ReadNextAsync();

                foreach (var item in result)
                {
                    sMember += "\n" + item.name;
                }
            } while (iterator.HasMoreResults);
            Sentence = sMember;
        }
        public static string Sentence { get; set; }
        public async Task UpdateMember()
        {
            var lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
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
                    }
                }
            } while (iterator.HasMoreResults);
        }
    }
}