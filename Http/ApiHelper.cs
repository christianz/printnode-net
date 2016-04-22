using System;
using System.CodeDom.Compiler;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PrintNode.Net
{
    internal static class ApiHelper
    {
        private const string BaseUri = "https://api.printnode.com";

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        internal static async Task<string> Get(string relativeUri)
        {
            using (var http = BuildHttpClient())
            {
                var result = await http.GetAsync(BaseUri + relativeUri, CancellationToken.None);

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception(result.StatusCode.ToString());
                }

                return await result.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> Post<T>(string relativeUri, T parameters)
        {
            using (var http = BuildHttpClient())
            {
                var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);

                var response = await http.PostAsync(BaseUri + relativeUri, new StringContent(json, Encoding.UTF8, "application/json"), CancellationToken.None);
                return await response.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> Patch<T>(string relativeUri, T parameters)
        {
            using (var http = BuildHttpClient())
            {
                var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BaseUri + relativeUri) { Content = new StringContent(json) };

                var response = await http.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static HttpClient BuildHttpClient()
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(PrintNodeConfiguration.GetApiKey())));
            http.DefaultRequestHeaders.Add("Accept-Version", "~3");

            return http;
        }
    }
}
