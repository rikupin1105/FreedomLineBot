using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FreedomLineBot
{
    public static class UpdateMemberName
    {
        [FunctionName("UpdateMemberName")]
        public static async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var db = new Database();
            await db.UpdateMember();
        }
    }
}
