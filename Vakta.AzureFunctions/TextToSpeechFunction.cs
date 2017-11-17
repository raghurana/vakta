using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Vakta.AzureFunctions
{
    public static class TextToSpeechFunction
    {
        [FunctionName("TextToSpeechFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "TextToSpeech/id/{id}/title/{title}")]
            HttpRequestMessage req, 
            string id,
            string title,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + id + " " + title);
        }
    }
}
