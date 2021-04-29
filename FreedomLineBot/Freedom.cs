using LineMessagingAPI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FreedomLineBot
{
    public class Freedom
    {
        public static MessageSender sender_admin = new MessageSender("管理メンバー", "https://raw.githubusercontent.com/rikupin1105/FreedomSite/master/IMG/admin.jpg");
        public static MessageSender sender_cat = new MessageSender("ฅ(=✧ω✧=)ฅﾆｬﾆｬｰﾝ✧", "https://raw.githubusercontent.com/rikupin1105/FreedomSite/master/IMG/cat.jpg");
        public static Database database { get; set; }
        public static List<string> Admin_Users = Environment.GetEnvironmentVariable("ADMIN_USER").Split(',').ToList();
        public static string groupId = Environment.GetEnvironmentVariable("GroupId");
    }
}