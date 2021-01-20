using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    class LineBotApp : WebhookApplication
    {
        private LineMessagingClient LineMessagingClient { get; set; }
        private string GroupID { get; set; }
        private List<string> Admin_Users { get; set; }
        private Database db { get; set; }
        public LineBotApp(LineMessagingClient lineMessagingClient)
        {
            LineMessagingClient = lineMessagingClient;
            db = new Database();
            Admin_Users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',').ToList();
            GroupID = Environment.GetEnvironmentVariable("GroupId");
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
        public MessageSender sender_admin = new MessageSender("管理メンバー", "https://raw.githubusercontent.com/rikupin1105/FreedomSite/master/IMG/admin.jpg");
        public MessageSender sender_cat = new MessageSender("ฅ(=✧ω✧=)ฅﾆｬﾆｬｰﾝ✧", "https://raw.githubusercontent.com/rikupin1105/FreedomSite/master/IMG/cat.jpg");
        protected override async Task OnMemberJoinAsync(MemberJoinEvent ev)
        {
            if (ev.Source.Id == GroupID)
            {
                var User_Name = LineMessagingClient.GetGroupMemberProfileAsync(ev.Source.Id, ev.Joined.Members[0].UserId);

                //入会時
                var messages = new ISendMessage[]
                {
                    new FlexMessage("こんにちは",sender_admin)
                    {
                        Contents = FlexMessageText.Flex_Greeting()
                    }
                };

                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);

                //CosmosDB
                await db.MemberAdd(new Member
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
            if (ev.Source.Id == GroupID)
            {
                //CosmosDB
                await db.MemberLeave(ev.Left.Members[0].UserId);

                var messages = new ISendMessage[]
                {
                    new TextMessage("退会されました。ブロック削除は個人の判断でお願いします。\n連絡先を貼ってください",null,sender_admin)
                };
                await LineMessagingClient.PushMessageAsync(ev.Source.Id, messages);
            }
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
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
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

                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
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

                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
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
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続確認リセット" && Admin_Users.Contains(ev.Source.UserId))
            {
                await db.MemberCheckReset();
                var messages = new ISendMessage[]
                {
                    new TextMessage("リセットしました",null,sender_admin)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await db.GetMember("SELECT c.newername FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await db.GetMember("SELECT c.name FROM c Where c.check != null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "希望済のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member,null,sender_admin)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続未希望メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await db.GetMember("SELECT c.newername FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.newername;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text == "継続未希望旧メンバー" && Admin_Users.Contains(ev.Source.UserId))
            {
                var member_list = await db.GetMember("SELECT c.name FROM c Where c.check = null and c.leavedDate = null ORDER BY c.joinedDate");
                string member = "未希望のメンバー";
                foreach (var item in member_list)
                {
                    member += "\n" + item.name;
                }
                var messages = new ISendMessage[]
                {
                    new TextMessage(member, null, sender_admin)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
            else if (msg.Text.Contains("にゃ") || msg.Text.Contains("ニャ"))
            {
                var rand = new Random();
                var catword = new string[] { "にゃฅ(｡•ㅅ•｡ฅ)", "(=ﾟ-ﾟ)ﾉﾆｬｰﾝ♪", "(=´∇｀=)にゃん", "ฅ(๑•̀ω•́๑)ฅﾆｬﾝﾆｬﾝｶﾞｵｰ", "ﾐｬｰ♪ヽ(∇⌒= )( =⌒∇)ﾉﾐｬｰ♪", "=^∇^*=　にゃお～ん♪" };
                var messages = new ISendMessage[]
                {
                    new TextMessage(catword[rand.Next(0, catword.Length)], null, sender_cat)
                };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
            }
        }

    }
}
