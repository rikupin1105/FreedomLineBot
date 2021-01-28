using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Threading.Tasks;

namespace FreedomLineBot
{
    public static class ContinueRequest
    {
        [FunctionName("ContinueRequest")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var LineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));

            string id = req.Query["ID"];
            var db = new Database();
            try
            {
                await db.MemberCheck(id);
                await LineMessagingClient.PushMessageAsync(id, "�p����]���m�F���܂���");
                return new OkObjectResult("");
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.ToString());
            }
        }
    }
}
