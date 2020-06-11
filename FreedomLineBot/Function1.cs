using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Line.Messaging;
using System.Net.Http;
using System.Net;
using Line.Messaging.Webhooks;
using System;

namespace FreedomLineBot
{
    public static class FreedomBot
    {

        [FunctionName("FreedomBot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req)
        {
            {
                try
                {
                    var channelSecret = Environment.GetEnvironmentVariable("CHANNEL_SEACRET");
                    var events = await req.GetWebhookEventsAsync(channelSecret);

                    var app = new LineBotApp();

                    await app.RunAsync(events);

                }
                catch (InvalidSignatureException e)
                {
                    return req.CreateResponse(HttpStatusCode.Forbidden, new { e.Message });
                }

                return req.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
