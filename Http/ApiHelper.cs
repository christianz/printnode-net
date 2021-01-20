using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    internal static class ApiHelper
    {
        private const string BaseUri = "https://api.printnode.com";
        private static readonly HttpClient Client = BuildHttpClient();

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static async Task<string> Get(string relativeUri)
        {
            var result = await Client.GetAsync(BaseUri + relativeUri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.StatusCode.ToString());
            }

            return await result.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Post<T>(string relativeUri, T parameters)
        {
            var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);

            var response = await Client.PostAsync(BaseUri + relativeUri, new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Patch<T>(string relativeUri, T parameters, string accountId)
        {
            var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), BaseUri + relativeUri) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            request.Headers.Add("X-Child-Account-By-Id", accountId);

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Delete(string relativeUri, string accountId)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), BaseUri + relativeUri);
            request.Headers.Add("X-Child-Account-By-Id", accountId);

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static HttpClient BuildHttpClient()
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(PrintNodeConfiguration.GetApiKey())));
            http.DefaultRequestHeaders.Add("Accept-Version", "~3");

            var context = PrintNodeDelegatedClientContext.Current;

            if (context != null)
            {
                var headerName = "";

                switch (context.AuthenticationMode)
                {
                    case PrintNodeDelegatedClientContextAuthenticationMode.Id:
                        headerName = "X-Child-Account-By-Id";
                        break;
                    case PrintNodeDelegatedClientContextAuthenticationMode.Email:
                        headerName = "X-Child-Account-By-Email";
                        break;
                    case PrintNodeDelegatedClientContextAuthenticationMode.CreatorRef:
                        headerName = "X-Child-Account-By-CreatorRef";
                        break;
                }

                http.DefaultRequestHeaders.Add(headerName, context.AuthenticationValue);
            }

            return http;
        }
    }
}
