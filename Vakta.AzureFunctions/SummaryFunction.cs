using System;
using System.Configuration;
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
    public static class SummaryFunction
    {
        public const int InvocationDelaysSecs = 12;

        [FunctionName("SummaryFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Summary")]
            HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var hit = await req.Content.ReadAsAsync<Hit>();

            var summaryApiUrl = ConfigurationManager.AppSettings["SummaryApiUrl"];
            var summaryApiKey = ConfigurationManager.AppSettings["SummaryApiKey"];

            var summaryService = new SummaryService(summaryApiUrl, summaryApiKey);
            var summary = summaryService.GetSummaryForHit(hit);
            
            var storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            
            var repo = new ArticleEntityRepo(storageConnectionString);

            repo.SaveArticle(new ArticleEntity(Guid.NewGuid(), hit.Title, hit.Url, summary));

            return req.CreateResponse(HttpStatusCode.OK, summary);
        }
    }
}
