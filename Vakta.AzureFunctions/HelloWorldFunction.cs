using System;
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

            foreach (var hit in hits)
            {
                var response = await InvokeSummaryFunction(req, hit);

               log.Info($"SummaryFunction Invoked and Result = {response.StatusCode}");
            }
            
            return req.CreateResponse(HttpStatusCode.OK);     
        }

        private static async Task<HttpResponseMessage> InvokeSummaryFunction(HttpRequestMessage request, Hit hit)
        {
            var result = await 
                request.InvokeAzureFunction(
                    "summary",
                    nameof(HelloWorldFunction),
                    hit);

            await Task.Delay(SummaryFunction.InvocationDelaysSecs);

            return result;
        }
    }
}
