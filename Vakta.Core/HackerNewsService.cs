using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;

namespace Vakta.Core
{
    public class HackerNewsService
    {
        private readonly IHackerNewsApi hackerApi;

        public HackerNewsService(string baseUrlPath = "https://hn.algolia.com/api/v1")
        {
            this.hackerApi = RestService.For<IHackerNewsApi>(baseUrlPath);
        }

        public async Task<IList<Hit>> GetTopFiveHackerNews()
        {
            var data = await hackerApi.GetFrontPageResults();

            return
                data
                    .Hits
                    .OrderByDescending(h => h.CreatedAt)
                    .Take(5)
                    .ToList();
        }
    }
}