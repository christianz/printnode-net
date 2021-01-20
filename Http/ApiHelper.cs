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
    internal static class ApiHelper
    {
        private const string BaseUri = "https://api.printnode.com";

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static async Task<string> Get(string relativeUri, PrintNodeRequestOptions options)
        {
            using (var http = BuildHttpClient(options))
            {
                var result = await http.GetAsync(BaseUri + relativeUri, CancellationToken.None);

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception(result.StatusCode.ToString());
                }

                return await result.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> Post<T>(string relativeUri, T parameters, PrintNodeRequestOptions options)
        {
            using (var http = BuildHttpClient(options))
            {
                var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);

                var response = await http.PostAsync(BaseUri + relativeUri, new StringContent(json, Encoding.UTF8, "application/json"), CancellationToken.None);

                if (!response.IsSuccessStatusCode)
                {
                    throw new PrintNodeException(response);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> Patch<T>(string relativeUri, T parameters, PrintNodeRequestOptions options, Dictionary<string, string> headers)
        {
            using (var http = BuildHttpClient(options, headers))
            {
                var json = JsonConvert.SerializeObject(parameters, DefaultSerializationSettings);
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BaseUri + relativeUri) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

                var response = await http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new PrintNodeException(response);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal static async Task<string> Delete(string relativeUri, PrintNodeRequestOptions options, Dictionary<string, string> headers)
        {
            using (var http = BuildHttpClient(options, headers))
            {
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), BaseUri + relativeUri);

                var response = await http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new PrintNodeException(response);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        private static HttpClient BuildHttpClient(PrintNodeRequestOptions options, Dictionary<string, string> headers = null)
        {
            var apiKey = options?.ApiKey ?? PrintNodeConfiguration.ApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("PrintNode API key not set! Please go to printnode.com and request an API key, and follow the instructions for configuring PrintNode.Net");
            }

            headers = headers ?? new Dictionary<string, string>();
            var http = new HttpClient();

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey)));
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

            foreach (var kv in headers)
            {
                http.DefaultRequestHeaders.Add(kv.Key, kv.Value);
            }

            return http;
        }
    }
}
