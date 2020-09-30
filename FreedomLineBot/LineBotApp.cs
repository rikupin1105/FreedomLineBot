using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    class LineBotApp : WebhookApplication
    {
        private LineMessagingClient LineMessagingClient { get; set; }
        private ILogger Log { get; set; }
        private string GroupID = Environment.GetEnvironmentVariable("GroupId");
        private string[] Admin_Users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',');
        public LineBotApp(LineMessagingClient lineMessagingClient, ILogger log)
        {
            LineMessagingClient = lineMessagingClient;
            Log = log;
        }
        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    Messaging(ev);
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
            if (ev.Source.Id == GroupID)
            {
                //入会時
                var User_Name = LineMessagingClient.GetGroupMemberProfileAsync(ev.Source.Id, ev.Joined.Members[0].UserId);
                var bubble = new FlexMessage("こんにちは") { Contents = FlexMessageText.Flex_Greeting() };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble });

                //CosmosDB
                var db = new Database();
                await db.MemberAdd(new Member
                {
                    id = ev.Joined.Members[0].UserId,
                    name = User_Name.Result.DisplayName,
                    newername = User_Name.Result.DisplayName,
                    joinedDate = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd h:mm")
                });

                Log.LogInformation("入会");
            }
        }
        protected override async Task OnMemberLeaveAsync(MemberLeaveEvent ev)
        {
            if (ev.Source.Id == GroupID)
            {
                await LineMessagingClient.PushMessageAsync(ev.Source.Id, "退会されました。ブロック削除は個人の判断でお願いします。\n連絡先を貼ってください");

                //CosmosDB
                var db = new Database();
                await db.MemberDelete(ev.Left.Members[0].UserId);

                Log.LogInformation("退会");
            }
        }
        private async void Messaging(MessageEvent ev)
        {
            if (!(ev.Message is TextEventMessage msg)) { return; }

            if (msg.Text.Contains("ルール"))
            {
                var bubble = new FlexMessage("ルール") { Contents = FlexMessageText.Flex_Rule() };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble });
                Log.LogInformation("ルール");
            }
            else if (msg.Text.Contains("FAQ"))
            {
                var bubble = new FlexMessage("FAQ") { Contents = FlexMessageText.Flex_Faq() };
                await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble });
                Log.LogInformation("FAQ");
            }
            else if (msg.Text == "継続希望")
            {
                var db = new Database();
                if (await db.MemberCheck(ev.Source.UserId) == true)
                {
                    var bubble = new FlexMessage("継続確認") { Contents = FlexMessageText.Flex_Continue_Checked(ev.Source.UserId.Substring(ev.Source.UserId.Length - 10)) };
                    await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble });
                    Log.LogInformation("継続確認");
                }
            }
            else if (msg.Text == "継続確認イベント")
            {
                var Admin_Users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',');
                foreach (string admin_user in Admin_Users)
                {
                    if (admin_user == ev.Source.UserId)
                    {
                        var bubble = new FlexMessage("継続確認イベント") { Contents = FlexMessageText.Flex_Check_Continue() };
                        await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble });
                        Log.LogInformation("継続確認イベント");
                        break;
                    }
                }
            }
            else if (msg.Text == "継続確認リセット")
            {
                var Admin_Users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',');
                foreach (string admin_user in Admin_Users)
                {
                    if (admin_user == ev.Source.UserId)
                    {
                        var db = new Database();
                        await db.MemberCheckReset();
                        await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, $"リセットしました");
                        break;
                    }
                }
            }
            else if (msg.Text == "継続希望メンバー")
            {
                foreach (string admin_user in Admin_Users)
                {
                    if (admin_user == ev.Source.UserId)
                    {
                        var db = new Database();
                        await db.GetMember("SELECT c.newername FROM c Where c.check != null");
                        await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, "継続希望メンバー\n敬称略、順不同\n" + Database.Sentence);
                        break;
                    }
                }

            }
            else if (msg.Text == "継続未希望メンバー")
            {
                foreach (string admin_user in Admin_Users)
                {
                    if (admin_user == ev.Source.UserId)
                    {
                        var db = new Database();
                        await db.GetMember("SELECT c.newername FROM c Where c.check = null");
                        await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, "継続未希望メンバー\n敬称略、順不同\n" + Database.Sentence);
                        break;
                    }
                }
            }
            else
            {
                var animalMessageList = new List<string>();
                if (msg.Text.Contains("にゃ") || msg.Text.Contains("ニャ"))
                {
                    var rand = new Random();
                    var catword = new string[] { "にゃฅ(｡•ㅅ•｡ฅ)?", "ฅ(=✧ω✧=)ฅﾆｬﾆｬｰﾝ✧", "(=ﾟ-ﾟ)ﾉﾆｬｰﾝ♪", "(=´∇｀=)にゃん", "ฅ(๑•̀ω•́๑)ฅﾆｬﾝﾆｬﾝｶﾞｵｰ", "ﾐｬｰ♪ヽ(∇⌒= )( =⌒∇)ﾉﾐｬｰ♪", "=^∇^*=　にゃお～ん♪" };

                    animalMessageList.Add(catword[rand.Next(0, catword.Length)]);
                }
                if (msg.Text.Contains("わん") || msg.Text.Contains("ワン"))
                {
                    var rand = new Random();
                    var dogword = new string[] { "ﾜﾝ(▼・ᴥ・▼)", "ﾜﾝ(U・ᴥ・U)", "ﾜﾝﾜﾝ(υ´•ﻌ•`υ)", "ﾜﾝ(U ･ㅊ･ U)?", "(U ･ˑ̫･ U)ﾜﾝ" };

                    animalMessageList.Add(dogword[rand.Next(0, dogword.Length)]);
                }
                if (msg.Text.Contains("ぴよ") || msg.Text.Contains("ピヨ"))
                {
                    var rand = new Random();
                    var chickword = new string[] { "ﾋﾟﾖ(•ө•)", "(ё) ピヨピヨ♪", "(ё)(ё)(ё) ピヨピヨ♪", "ﾄ(･Θ･)ﾋﾟﾖﾋﾟﾖ♪", "(*・Θ・*)ﾋﾟﾖｯ" };

                    animalMessageList.Add(chickword[rand.Next(0, chickword.Length)]);
                }

                if (animalMessageList.Count != 0)
                {
                    var mes = string.Join('\n', animalMessageList);
                    await LineMessagingClient.ReplyMessageAsync(ev.ReplyToken, mes);
                    Log.LogInformation("動物");
                }
            }
        }

    }
}
