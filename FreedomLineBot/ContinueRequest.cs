using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Threading.Tasks;
using static FreedomLineBot.Freedom;

namespace FreedomLineBot
{
    public static class ContinueRequest
    {
        [FunctionName("ContinueRequest")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));

            string id = req.Query["ID"];
            var db = new Database();
            try
            {
                await db.MemberCheck(id);
                var mes = new ISendMessage[]
                {
                    new TextMessage("Œp‘±Šó–]‚ðŠm”F‚µ‚Ü‚µ‚½")
                };
                await lineMessagingClient.PushMessageAsync(id, mes);
                return new OkObjectResult("");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.ToString());
            }
        }
    }
}
