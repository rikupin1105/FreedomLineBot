using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Threading.Tasks;
using static FreedomLineBot.Freedom;

namespace FreedomLineBot
{
    class LineBotApp : WebhookApplication
    {
        private LineMessagingClient lineMessagingClient { get; set; }
        public LineBotApp()
        {
            lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN")); ;
            database = new Database();
        }
        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await Messaging(ev);
                    break;

                case EventMessageType.Image:
                case EventMessageType.Audio:
                case EventMessageType.Video:
                case EventMessageType.File:
                case EventMessageType.Location:
                case EventMessageType.Sticker:
                    break;
            }
        }

        protected override async Task OnMemberJoinAsync(MemberJoinEvent ev)
        {
            if (ev.Source.Id == groupId)
            {
                var User_Name = lineMessagingClient.GetGroupMemberProfileAsync(ev.Source.Id, ev.Joined.Members[0].UserId);

                //入会時
                var messages = new ISendMessage[]
                {
                    new FlexMessage("こんにちは",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Greeting()
                    }
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);

                //CosmosDB
                await database.MemberAdd(new Member
                {
                    id = ev.Joined.Members[0].UserId,
                    name = User_Name.Result.DisplayName,
                    newername = User_Name.Result.DisplayName,
                    joinedDate = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd h:mm"),
                    leavedDate = null
                });
            }
        }
        protected override async Task OnMemberLeaveAsync(MemberLeaveEvent ev)
        {
            if (ev.Source.Id == groupId)
            {
                //CosmosDB
                await database.MemberLeave(ev.Left.Members[0].UserId);
                var mes1 = new ISendMessage[]
                {
                    new TextMessage("グループに参加していただきありがとうございました。このBOTはブロック削除をしてください。",null,sender_admin)
                };
                var mes2 = new ISendMessage[]
                {
                    new TextMessage("誰かが退会しました",null,sender_admin)
                };

                await lineMessagingClient.PushMessageAsync(ev.Left.Members[0].UserId, mes1);
                await lineMessagingClient.PushMessageAsync(Environment.GetEnvironmentVariable("ADMIN_GROUP"), mes2);
            }
        }
        public async Task ContinueVerification(string Id)
        {
            var mes = new ISendMessage[]
            {
                new TextMessage("継続希望を確認しました",null,sender_admin)
            };
            await lineMessagingClient.PushMessageAsync(Id, mes);
        }
        private async Task Messaging(MessageEvent ev)
        {
            if (!(ev.Message is TextEventMessage msg)) { return; }

            if (msg.Text.Contains("ルール") && msg.Text.Contains("FAQ"))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("ルール",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Rule()
                    },
                    new FlexMessage("FAQ",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Faq()
                    }
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("ルール"))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("ルール",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Rule()
                    }
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("FAQ"))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("FAQ",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Faq()
                    }
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続確認イベント" && Admin_Users.Contains(ev.Source.UserId))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("継続確認イベント",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Check_Continue()
                    }
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続確認リセット" && Admin_Users.Contains(ev.Source.UserId))
            {
                await database.MemberCheckReset();
                var messages = new ISendMessage[]
                {
                    new TextMessage("リセットしました",null,sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await database.GetMember("SELECT c.newername FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await database.GetMember("SELECT c.name FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.name;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member,null,sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続未希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await database.GetMember("SELECT c.newername FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続未希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await database.GetMember("SELECT c.name FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.name;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("にゃ") || msg.Text.Contains("ニャ"))
            {
                var rand = new Random();
                var catword = new string[] { "にゃฅ(｡•ㅅ•｡ฅ)", "(=ﾟ-ﾟ)ﾉﾆｬｰﾝ♪", "(=´∇｀=)にゃん", "ฅ(๑•̀ω•́๑)ฅﾆｬﾝﾆｬﾝｶﾞｵｰ", "ﾐｬｰ♪ヽ(∇⌒= )( =⌒∇)ﾉﾐｬｰ♪", "=^∇^*=　にゃお～ん♪" };
                var messages = new ISendMessage[]
                {
                    new TextMessage(catword[rand.Next(0, catword.Length)], null, sender_cat)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "プッシュ")
            {
                await lineMessagingClient.PushMessageAsync(ev.Source.UserId, "継続希望を確認しました！");
            }
        }
    }
}
