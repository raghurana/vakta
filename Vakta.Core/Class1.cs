using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;

namespace Vakta.Core
{
    public class Class1
    {
        public async Task<IList<Hit>> GetItems(int count)
        {
            var hackerApi = RestService.For<IHackerNewsApi>("https://hn.algolia.com/api/v1");
            var data      = await hackerApi.GetFrontPageResults();

            return 
                data
                    .Hits
                    .OrderByDescending(h => h.CreatedAtI)
                    .Take(5)
                    .ToList();
        }
    }
}
