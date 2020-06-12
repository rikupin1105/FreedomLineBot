using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
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

            if (msg.Text.Contains("にゃ") && msg.Text != "ルールにゃ" && msg.Text != "FAQにゃ")
            {
                var rand = new Random();
                var catword = new string[] { "にゃฅ(｡•ㅅ•｡ฅ)?", "ฅ(=✧ω✧=)ฅﾆｬﾆｬｰﾝ✧", "(=ﾟ-ﾟ)ﾉﾆｬｰﾝ♪", "(=´∇｀=)にゃん" };
                await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, catword[rand.Next(0,catword.Length)]);
            }
            else if (msg.Text == "ルール")
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
        }

    }
}
