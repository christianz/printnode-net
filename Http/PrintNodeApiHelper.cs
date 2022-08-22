using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNodeNet.Http
{
    internal static class PrintNodeApiHelper
    {
        private static readonly HttpClient Client = BuildHttpClient();

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static async Task<string> Get(string relativeUri, PrintNodeRequestOptions options)
        {
            SetAuthenticationHeader(Client, options);

            var result = await Client.GetAsync(relativeUri, CancellationToken.None);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.StatusCode.ToString());
            }

            return await result.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Post<T>(string relativeUri, T parameters, PrintNodeRequestOptions options)
        {
            SetAuthenticationHeader(Client, options);

            var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);

            var response = await Client.PostAsync(relativeUri, new StringContent(json, Encoding.UTF8, "application/json"), CancellationToken.None);

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Patch<T>(string relativeUri, T parameters, PrintNodeRequestOptions options, Dictionary<string, string> headers)
        {
            SetAuthenticationHeader(Client, options);

            var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), relativeUri) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            
            foreach (var h in headers)
            {
                request.Headers.Add(h.Key, h.Value);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        internal static async Task<string> Delete(string relativeUri, PrintNodeRequestOptions options, Dictionary<string, string> headers)
        {
            SetAuthenticationHeader(Client, options);

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), relativeUri);

            foreach (var h in headers)
            {
                request.Headers.Add(h.Key, h.Value);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new PrintNodeException(response);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static void SetAuthenticationHeader(HttpClient client, PrintNodeRequestOptions options)
        {
            var apiKey = options?.ApiKey ?? PrintNodeConfiguration.ApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("PrintNode API key not set! Please go to printnode.com and request an API key, and follow the instructions for configuring PrintNode.Net");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey)));
        }

        private static HttpClient BuildHttpClient()
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("Accept-Version", "~3");
            http.BaseAddress = PrintNodeConfiguration.BaseAddress;

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
