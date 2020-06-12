using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    class LineBotApp : WebhookApplication
    {
        private LineMessagingClient lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
        private GoogleSpreadSheet GAS = new GoogleSpreadSheet();

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
            //入会時
            var User_Name = lineMessagingClient.GetGroupMemberProfileAsync(ev.Source.Id, ev.Joined.Members[0].UserId);
            GAS.Join(ev.Joined.Members[0].UserId, User_Name.Result.DisplayName);
            await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonGreeting);
        }
        protected override async Task OnMemberLeaveAsync(MemberLeaveEvent ev)
        {
            //退会時
            GAS.Leave(ev.Left.Members[0].UserId);
            await lineMessagingClient.PushMessageAsync(ev.Source.Id, "退会されました。ブロック削除は個人の判断でお願いします。\n連絡先を貼ってください");
        }
        private async void Messaging(MessageEvent ev)
        {
            if (!(ev.Message is TextEventMessage msg)) { return; }

            if (msg.Text == "ルール")
            {
                await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonRules);
            }
            else if (msg.Text == "ルールにゃ")
            {
                await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonRulesWithCat);
            }
            else if (msg.Text == "FAQ")
            {
                await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonFAQs);
            }
            else if (msg.Text == "FAQにゃ")
            {
                await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonFAQsWithCat);
            }
            else if (msg.Text == "継続希望")
            {
                GAS.Continue(ev.Source.UserId);
                var User_Name = lineMessagingClient.GetGroupMemberProfileAsync(ev.Source.Id, ev.Source.UserId).Result.DisplayName;
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, User_Name + "さん 継続希望確認しました。");
            }
            else if (msg.Text == "継続確認イベント")
            {
                var admin_users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',');
                foreach (string admin_user in admin_users)
                {
                    if (admin_user == ev.Source.UserId)
                    {
                        await lineMessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexMessageText.FlexJsonCheckContinue);
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
                    await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, mes);
                }
            }
        }

    }
}
