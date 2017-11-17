using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Vakta.Core;

namespace Vakta.AzureFunctions
{
    public static class HelloWorldFunction
    {
        [FunctionName(nameof(HelloWorldFunction))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequestMessage req, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var hackerNews = new HackerNewsService();

            var hits = await hackerNews.GetTopFiveHackerNews();

            var tasks = hits.Select(hit => InvokeSummaryFunction(req, hit)).ToList();

            var allResponses = await Task.WhenAll(tasks);
            
            // TODO: Check all responses to be ok

            return req.CreateResponse(HttpStatusCode.OK);     
        }

        private static Task<HttpResponseMessage> InvokeSummaryFunction(HttpRequestMessage request, Hit hit)
        {
            return
                request.InvokeAzureFunction(
                    "summary",
                    nameof(HelloWorldFunction),
                    hit);
        }
    }
}
