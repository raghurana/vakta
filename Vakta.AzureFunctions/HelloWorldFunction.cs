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

            var results = await hackerNews.GetTopFiveHackerNews();

            var baseUrl = req.RequestUri.AbsoluteUri.TrimEnd(nameof(HelloWorldFunction).ToCharArray());

            using (var functionClient = new HttpClient())
            {
                var test = await functionClient.PostAsJsonAsync($"{baseUrl}summary", results.First());
            }
            
            return req.CreateResponse(HttpStatusCode.OK, results);     
        }
    }
}
