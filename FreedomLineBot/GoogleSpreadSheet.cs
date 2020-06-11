using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FreedomLineBot
{
    class GoogleSpreadSheet
    {
        private string URL = Environment.GetEnvironmentVariable("SpreadsheetURL");
        private void Upload(Gas parameter)
        {
            var json = JsonSerializer.Serialize(parameter);
            var wc = new WebClient();
            wc.UploadString(URL, json);
        }
        /// <summary>
        /// スプレッドシートに退会日を記述します。
        /// </summary>
        /// <param name="UserId"></param>
        public void Leave(string UserId)
        {
            var parameter = new Gas
            {
                Date = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd h:mm"),
                UserID = UserId,
                Option = "leave"
            };
            Upload(parameter);
        }
        /// <summary>
        /// スプレッドシートにユーザーID、名前、参加日を記述します。
        /// 再参加の場合は退会日の記述を削除します。
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="userName"></param>
        public void Join(string UserId, string userName)
        {
            var parameter = new Gas
            {
                UserID = UserId,
                Date = DateTime.UtcNow.AddHours(9).ToString("yyyy/MM/dd h:mm"),
                UserName = userName,
                Option = "join"
            };
            Upload(parameter);
        }
        /// <summary>
        /// 継続希望の欄に済を記述します
        /// </summary>
        /// <param name="userID"></param>
        public void Continue(string userID)
        {
            var parameter = new Gas
            {
                UserID = userID,
                Option = "continue"
            };
            Upload(parameter);
        }
        public class Gas
        {
            [JsonPropertyName("userID")]
            public string UserID { get; set; }
            [JsonPropertyName("userName")]
            public string UserName { get; set; }
            [JsonPropertyName("data")]
            public string Date { get; set; }
            /// <summary>
            /// 継続希望の場合はcontinue
            /// 参加の場合はjoin
            /// 退会の場合はleave
            /// </summary>
            [JsonPropertyName("option")]
            public string Option { get; set; }
        }
    }
}

