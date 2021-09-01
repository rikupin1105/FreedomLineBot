using LineMessagingAPI;
using LineMessagingAPI.Webhooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FreedomLineBot.Freedom;

namespace FreedomLineBot
{
    internal class LineBotApp : WebhookApplication
    {
        private LineMessagingClient lineMessagingClient { get; set; }
        public LineBotApp()
        {
            lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
            Freedom.Database = new Database();
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
                    new FlexMessage("こんにちは",FlexMessageText.Flex_Greeting(),messageSender:sender_admin)
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);

                //CosmosDB
                await Freedom.Database.MemberAdd(new Member
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
                await Freedom.Database.MemberLeave(ev.Left.Members[0].UserId);
                var mes1 = new ISendMessage[]
                {
                    new TextMessage("グループに参加していただきありがとうございました。このBOTはブロック削除をしてください。",messageSender:sender_admin)
                };
                var mes2 = new ISendMessage[]
                {
                    new TextMessage("誰かが退会しました",messageSender:sender_admin)
                };

                await lineMessagingClient.PushMessageAsync(ev.Left.Members[0].UserId, mes1);
                await lineMessagingClient.PushMessageAsync(Environment.GetEnvironmentVariable("ADMIN_GROUP"), mes2);
            }
        }
        public async Task ContinueVerification(string Id)
        {
            var mes = new ISendMessage[]
            {
                new TextMessage("継続希望を確認しました",messageSender:sender_admin)
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
                    new FlexMessage("ルール",FlexMessageText.Flex_Rule(),messageSender:sender_admin),
                    new FlexMessage("FAQ",FlexMessageText.Flex_Faq(),messageSender:sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("ルール"))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("ルール",FlexMessageText.Flex_Rule(),messageSender:sender_admin)
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("FAQ"))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("FAQ",FlexMessageText.Flex_Faq(),messageSender:sender_admin)
                };

                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続確認イベント" && Admin_Users.Contains(ev.Source.UserId))
            {
                var messages = new ISendMessage[]
                {
                    new FlexMessage("継続確認イベント",FlexMessageText.Flex_Check_Continue(),messageSender:sender_admin)
                };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続確認リセット" && Admin_Users.Contains(ev.Source.UserId))
            {
                await Freedom.Database.MemberCheckReset();
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, "リセットしました", messageSender: sender_admin);
            }
            else if (msg.Text == "継続希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await Freedom.Database.GetMember("SELECT c.newername FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, member, messageSender: sender_admin);
            }
            else if (msg.Text == "継続希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await Freedom.Database.GetMember("SELECT c.name FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.name;
                }
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, member, messageSender: sender_admin);
            }
            else if (msg.Text == "継続未希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await Freedom.Database.GetMember("SELECT c.newername FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, member, messageSender: sender_admin);
            }
            else if (msg.Text == "継続未希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await Freedom.Database.GetMember("SELECT c.name FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.name;
                }
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, member, messageSender: sender_admin);
            }
            else if (msg.Text.Contains("にゃ") || msg.Text.Contains("ニャ"))
            {
                var rand = new Random();
                var catword = new string[] { "にゃฅ(｡•ㅅ•｡ฅ)", "(=ﾟ-ﾟ)ﾉﾆｬｰﾝ♪", "(=´∇｀=)にゃん", "ฅ(๑•̀ω•́๑)ฅﾆｬﾝﾆｬﾝｶﾞｵｰ", "ﾐｬｰ♪ヽ(∇⌒= )( =⌒∇)ﾉﾐｬｰ♪", "=^∇^*=　にゃお～ん♪" };
                await lineMessagingClient.ReplyTextAsync(ev.ReplyToken, catword[rand.Next(0, catword.Length)], false,null, sender_cat);
            }
        }
    }
}