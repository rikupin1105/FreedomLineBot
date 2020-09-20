using Line.Messaging;

namespace FreedomLineBot
{
    class FlexMessageText
    {
        public static BubbleContainer Flex_Check_Continue()
        {
            return new BubbleContainer()
            {
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "継続希望しますか？",
                            Weight = Weight.Bold,
                            Size = ComponentSize.Xl
                        },
                        new BoxComponent()
                        {
                            Layout = BoxLayout.Baseline,
                            Spacing = Spacing.Sm,
                            Contents = new IFlexComponent[]
                            {
                                new TextComponent()
                                {
                                    Text = "期限",
                                    Size = ComponentSize.Sm,
                                    Color = ColorCode.FromRgb(102, 102, 102)
                                },
                                new TextComponent()
                                {
                                    Text = "日曜日 22:00 まで",
                                    Size = ComponentSize.Sm,
                                    Wrap = true,
                                    Color = ColorCode.FromRgb(102, 102, 102)
                                }
                            }
                        },
                        new TextComponent()
                        {
                            Text = "BOTの反応は数秒から数十秒遅れることがあります。何度も押す必要はありませんが、数分経っても返答がない場合は再度押していただくか、管理メンバーに連絡してください。",
                            Wrap = true,
                            Size = ComponentSize.Sm,
                            Color = ColorCode.FromRgb(102, 102, 102)
                        },
                        new TextComponent()
                        {
                            Text = "実施期間終了の前日、または前々日に意思表示をされていないメンバーさん宛にメンションをつけて、管理メンバーがGTにてご連絡をします。実施期間終了後、継続を希望しないメンバーさんを、管理メンバーが退会大会に出場させます。ブロック非表示削除等は、各自自由に判断してください。",
                            Wrap = true,
                            Size = ComponentSize.Sm,
                            Color = ColorCode.FromRgb(102, 102, 102)
                        }
                    }
                },
                Footer = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new ButtonComponent()
                        {
                            Action = new MessageTemplateAction("継続希望","継続確認"),
                            Style = ButtonStyle.Primary,
                            Height = ButtonHeight.Sm
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("ルールを見る","https://rikupin.github.io/FreedomSite/Rules"),
                            Style = ButtonStyle.Secondary,
                            Height = ButtonHeight.Sm
                        }
                    }
                }

            };
        }
        public static BubbleContainer Flex_Greeting()
        {
            return new BubbleContainer()
            {
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "こんにちは",
                            Weight = Weight.Bold,
                            Size = ComponentSize.Xl
                        },
                        new TextComponent()
                        {
                            Text = "ルールを確認、メンバー全員を登録してください。当Botは登録不要です。",
                            Wrap = true,
                            Size = ComponentSize.Md,
                            Color = ColorCode.FromRgb(102,102,102)
                        }
                    }
                },
                Footer = new BoxComponent()
                {
                    Layout = BoxLayout.Horizontal,
                    Spacing = Spacing.Sm,
                    Contents = new IFlexComponent[]
                    {
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("ルール","https://rikupin.github.io/FreedomSite/Rules"),
                            Style = ButtonStyle.Primary,
                            Height = ButtonHeight.Sm
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("FAQ","https://rikupin.github.io/FreedomSite/FAQs"),
                            Style = ButtonStyle.Secondary,
                            Height = ButtonHeight.Sm,
                            Margin = Spacing.Md
                        }
                    }
                }

            };
        }
        public static BubbleContainer Flex_Faq()
        {
            return new BubbleContainer()
            {
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "いま一度FAQを確認してくださいφ(>ω<*)☆",
                            Size = ComponentSize.Md,
                            Wrap = true
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("FAQを見る","https://rikupin.github.io/FreedomSite/FAQs"),
                            Style = ButtonStyle.Secondary,
                            Height = ButtonHeight.Sm
                        }
                    }
                }
            };
        }
        public static BubbleContainer Flex_Rule()
        {
            return new BubbleContainer()
            {
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "いま一度ルールを確認してくださいφ(>ω<*)☆",
                            Size = ComponentSize.Md,
                            Wrap = true
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("ルールを見る","https://rikupin.github.io/FreedomSite/Rules"),
                            Style = ButtonStyle.Secondary,
                            Height = ButtonHeight.Sm
                        }
                    }
                }
            };
        }
        public static BubbleContainer Flex_Continue_Checked(string ID)
        {
            return new BubbleContainer()
            {
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Action = new UriTemplateAction("ルールを見る", "https://rikupin.github.io/FreedomSite/Rules"),
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "継続希望を確認しました。"
                        },
                        new TextComponent()
                        {
                            Text = $"管理用 : {ID}",
                            Size = ComponentSize.Sm
                        }
                    }
                }
            };
        }
    }
}
