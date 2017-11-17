using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Vakta.Core
{
    public class SummaryService
    {
        private readonly string apiUrl;
        private readonly string apiKey;

        public SummaryService(string apiUrl, string apiKey)
        {
            this.apiUrl = apiUrl;
            this.apiKey = apiKey;
        }

        public string GetSummaryForHit(Hit hit)
        {
            var summaryApiUrl = string.Format(apiUrl, apiKey, WebUtility.UrlEncode(hit.Url));

            string summary;
            using (var wc = new WebClient())
            {
                summary = wc.DownloadString(summaryApiUrl);
            }

            if (string.IsNullOrEmpty(summary))
                throw new Exception("Summary is empty");

            var jo = JObject.Parse(summary);

            return $"{jo.SelectToken("['sm_api_title']")}.{jo.SelectToken("['sm_api_content']")}";
        }
    }
}
