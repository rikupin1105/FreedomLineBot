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
                                    Color = ColorCode.FromRgb(102, 102, 102),
                                    Align = Align.End
                                }
                            }
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
                    Spacing = Spacing.Sm,
                    Contents = new IFlexComponent[]
                    {
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("希望する","https://liff.line.me/1655282180-Le1b0wWq"),
                            Style = ButtonStyle.Primary,
                            Height = ButtonHeight.Sm
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("ルールを見る","https://rikupin1105.github.io/FreedomSite/Rules"),
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
                            Text = "いらっしゃいませー",
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
                            Action = new UriTemplateAction("ルール","https://rikupin1105.github.io/FreedomSite/Rules"),
                            Style = ButtonStyle.Primary,
                            Height = ButtonHeight.Sm
                        },
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("FAQ","https://rikupin1105.github.io/FreedomSite/FAQs"),
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
                Size = BubbleSize.micro,
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("FAQを見る", "https://rikupin1105.github.io/FreedomSite/FAQs"),
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
                Size = BubbleSize.micro,
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new ButtonComponent()
                        {
                            Action = new UriTemplateAction("ルールを見る", "https://rikupin1105.github.io/FreedomSite/Rules"),
                            Style = ButtonStyle.Secondary,
                            Height = ButtonHeight.Sm
                        }
                    }
                }
            };
        }
    }
}
