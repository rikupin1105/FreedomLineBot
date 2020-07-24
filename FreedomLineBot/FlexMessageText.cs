namespace FreedomLineBot
{
    class FlexMessageText
    {
        public static string FlexJsonCheckContinue =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""text"",
          ""text"": ""継続希望しますか?"",
          ""size"": ""xl"",
          ""weight"": ""bold""
        },
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""期限"",
              ""flex"": 1,
              ""size"": ""sm"",
              ""color"": ""#aaaaaa""
            },
            {
              ""type"": ""text"",
              ""text"": ""日曜日 22:00 まで"",
              ""flex"": 5,
              ""size"": ""sm"",
              ""wrap"": true,
              ""color"": ""#666666""
            }
          ],
          ""spacing"": ""sm""
        },
        {
              ""type"": ""text"",
              ""text"": ""BOTの反応は数秒から数十秒遅れることがあります。何度も押す必要はありませんが、数分経っても返答がない場合は再度押していただくか、管理メンバーに連絡してください。"",
              ""flex"": 5,
              ""wrap"": true,
              ""size"": ""sm"",
              ""color"": ""#666666""
            
        },
        {
              ""type"": ""text"",
              ""text"": ""実施期間終了の前日、または前々日に意思表示をされていないメンバーさん宛にメンションをつけて、管理メンバーがGTにてご連絡をします。実施期間終了後、継続を希望しないメンバーさんを、管理メンバーが退会大会に出場させます。ブロック非表示削除等は、各自自由に判断してください。"",
              ""flex"": 5,
              ""wrap"": true,
              ""size"": ""sm"",
              ""color"": ""#666666""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""button"",
          ""action"": {
            ""type"": ""message"",
            ""label"": ""継続希望"",
            ""text"":""継続希望""
          },
          ""height"": ""sm"",
          ""style"": ""primary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""継続確認""
}";
        public static string FlexJsonGreeting =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""text"",
          ""text"": ""こんにゃちはฅ(｡•ㅅ•｡ฅ"",
          ""size"": ""xl"",
          ""weight"": ""bold""
        },
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""ルールを確認、メンバー全員を登録してください。当botは登録不要です。"",
              ""flex"": 5,
              ""size"": ""md"",
              ""wrap"": true,
              ""color"": ""#666666""
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""ルールを見る"",
            ""uri"":""https://freedom-site.netlify.app/Rules""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        },
{
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""FAQを見る"",
            ""uri"":""https://freedom-site.netlify.app/FAQs""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""こんにちは""
}";
        public static string FlexJsonFAQs =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""いま一度FAQを確認してくださいφ(>ω<*)☆"",
              ""flex"": 5,
              ""size"": ""md"",
              ""wrap"": true
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
{
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""FAQを見る"",
            ""uri"":""https://freedom-site.netlify.app/FAQs""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""FAQ""
}";
        public static string FlexJsonRules =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""いま一度ルールを確認してくださいφ(> ω < *)☆"",
              ""flex"": 5,
              ""size"": ""md"",
              ""wrap"": true
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
{
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""ルールを見る"",
            ""uri"":""https://freedom-site.netlify.app/Rules""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""Rules""
}";
        public static string FlexJsonFAQsWithCat =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""いま一度FAQを確認してくださいにゃ"",
              ""flex"": 5,
              ""size"": ""md"",
              ""wrap"": true
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
{
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""FAQを見る"",
            ""uri"":""https://freedom-site.netlify.app/FAQs""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""FAQ""
}";
        public static string FlexJsonRulesWithCat =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""いま一度ルールを確認してくださいにゃ"",
              ""flex"": 5,
              ""size"": ""md"",
              ""wrap"": true
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
{
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""ルールを見る"",
            ""uri"":""https://freedom-site.netlify.app/Rules""
          },
          ""height"": ""sm"",
          ""style"": ""secondary""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""Rules""
}";
    }
}
