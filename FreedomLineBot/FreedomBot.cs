using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static FreedomLineBot.Freedom;

namespace FreedomLineBot
{
    public static class FreedomBot
    {
        [FunctionName("FreedomBot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req, ILogger log)
        {
            {
                try
                {
                    log.LogInformation(req.Content.ReadAsStringAsync().Result);
                    var channelSecret = Environment.GetEnvironmentVariable("CHANNEL_SEACRET");
                    lineMessagingClient = new LineMessagingClient(Environment.GetEnvironmentVariable("CHANNEL_ACCESS_TOKEN"));
                    var events = await req.GetWebhookEventsAsync(channelSecret);

                    var app = new LineBotApp(lineMessagingClient);

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
