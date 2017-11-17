using System.Net.Http;
using System.Threading.Tasks;

namespace Vakta.AzureFunctions
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<HttpResponseMessage> InvokeAzureFunction(this HttpRequestMessage request, string targetFunctionRoute, string invokingFunctionName, object data = null)
        {
            var baseUrl = request.RequestUri.AbsoluteUri.TrimEnd(invokingFunctionName.ToCharArray());

            using (var functionClient = new HttpClient())
            {
                return await functionClient.PostAsJsonAsync($"{baseUrl}{targetFunctionRoute}", data);
            }
        }
    }
}
