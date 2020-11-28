using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace FreedomLineBot
{
    public static class ContinueRequest
    {
        [FunctionName("ContinueRequest")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string ID = req.Query["ID"];
            var db = new Database();
            await db.MemberCheck(ID);
            return new OkObjectResult(ID);
        }
    }
}
