using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Refit;

namespace Vakta.Core
{
    public class Class1
    {
        private const string BasePath = @"c:\temp";

        public async Task<Dictionary<Hit, string>> Run()
        {
            var top5 = await GetTopFiveHackerNews();

            var result = new Dictionary<Hit, string>();

            foreach (var hit in top5)
            {
                var summary = await GetSummaryForHit(hit);
                var saveLocation = Path.Combine(BasePath, $"{hit.ObjectID}.mp3");
                await TextToMp3(summary, saveLocation);

                result.Add(hit, saveLocation);    
            }

            return result;
        }

        private async Task<IList<Hit>> GetTopFiveHackerNews()
        {
            var hackerApi = RestService.For<IHackerNewsApi>("https://hn.algolia.com/api/v1");
            var data = await hackerApi.GetFrontPageResults();

            return
                data
                    .Hits
                    .OrderByDescending(h => h.CreatedAtI)
                    .Take(2)
                    .ToList();
        }

        private Task<string> GetSummaryForHit(Hit hit)
        {
            return Task.FromResult(hit.Title);
        }

        private async Task TextToMp3(string text, string mp3FilePath)
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
}
