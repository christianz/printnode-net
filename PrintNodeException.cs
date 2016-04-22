using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public class PrintNodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public PrintNodeException(HttpResponseMessage response) : base(GetErrorContent(response))
        {
            StatusCode = response.StatusCode;
        }

        private static string GetErrorContent(HttpResponseMessage response)
        {
            var message = response.Content.ReadAsStringAsync().Result;
            var content = JsonConvert.DeserializeObject<dynamic>(message);

            return content.message;
        }
    }
}
