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
            string id = req.Query["ID"];
            var db = new Database();
            try
            {
                await db.MemberCheck(id);
                await new LineBotApp().ContinueVerification(id);
                return new OkObjectResult("");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.ToString());
            }
        }
    }
}
