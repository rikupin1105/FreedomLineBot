using LineMessagingAPI;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
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

        //継続希望を記入する
        public async Task MemberCheck(string Id)
        {
            var responce = await container.ReadItemAsync<Member>(Id, new PartitionKey("freedom"));
            await container.UpsertItemAsync(new Member
            {
                id = Id,
                name = responce.Resource.name,
                newername = responce.Resource.newername,
                joinedDate = responce.Resource.joinedDate,
                check = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd HH:mm"),
                postScript = responce.Resource.postScript
            });
        }

        //メンバーが退会した際データベースを更新する
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

        //入会時にデータベースを更新する
        public async Task MemberAdd(Member m)
        {
            await container.UpsertItemAsync(m);
        }

        //継続希望をリセットする
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

        //クエリからメンバーの名前を取得する
        public async Task<List<Member>> GetMember(string query)
        {
            var member_list = new List<Member>();
            var iterator = container.GetItemQueryIterator<Member>(query);
            do
            {
                var result = await iterator.ReadNextAsync();
                foreach (var item in result)
                {
                    member_list.Add(item);
                }
            } while (iterator.HasMoreResults);
            return member_list;
        }

        //メンバーの名前を更新
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