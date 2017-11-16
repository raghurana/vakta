using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Newtonsoft.Json.Linq;
using Refit;

namespace Vakta.Core
{
    public class Class1
    {
        public static async Task<Dictionary<Hit, string>> Run(RunOptions options)
        {
            var top5   = await GetTopFiveHackerNews();
            var result = new Dictionary<Hit, string>();

            foreach (var hit in top5)
            {
                var summary = GetSummaryForHit(hit, options);
                var saveLocation = Path.Combine(options.Mp3OutputFolder, $"{hit.ObjectID}.mp3");
                await TextToMp3(summary, saveLocation);

                result.Add(hit, saveLocation);    
            }

            return result;
        }

        private static async Task<IList<Hit>> GetTopFiveHackerNews()
        {
            var hackerApi = RestService.For<IHackerNewsApi>("https://hn.algolia.com/api/v1");
            var data = await hackerApi.GetFrontPageResults();

            return
                data
                    .Hits
                    .OrderByDescending(h => h.CreatedAtI)
                    .Take(5)
                    .ToList();
        }

        private static string GetSummaryForHit(Hit hit, RunOptions options)
        {
            var apiUrl = string.Format(options.SummaryApiUrl, options.SummaryApiKey, WebUtility.UrlEncode(hit.Url));

            string summary;    
            using (var wc = new WebClient())
            {
                summary = wc.DownloadString(apiUrl);
            }

            if (string.IsNullOrEmpty(summary))
                throw new Exception("Summary is empty");

            var jo = JObject.Parse(summary);
            return $"{jo.SelectToken("['sm_api_title']")}.{jo.SelectToken("['sm_api_content']")}";
        }

        private static async Task TextToMp3(string text, string mp3FilePath)
        {
            using (var pollyClient = new AmazonPollyClient(RegionEndpoint.APSoutheast2))
            {
                var sreq = new SynthesizeSpeechRequest
                {
                    Text = text,
                    OutputFormat = OutputFormat.Mp3,
                    VoiceId = VoiceId.Amy
                };

                var sres = await pollyClient.SynthesizeSpeechAsync(sreq);
               
                using (var fileStream = File.Create(mp3FilePath))
                {
                    sres.AudioStream.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
        }
    }

    public class RunOptions
    {
        public string Mp3OutputFolder { get; }
        
        public string SummaryApiUrl { get; }

        public string SummaryApiKey { get; }

        public RunOptions(string mp3OutputFolder, string summaryApiUrl, string summaryApiKey)
        {
            Mp3OutputFolder = mp3OutputFolder;
            SummaryApiUrl = summaryApiUrl;
            SummaryApiKey = summaryApiKey;
        }
    }
}
