using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNodeNet
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
            var message = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
            var content = JsonConvert.DeserializeObject<PrintNodeErrorMessage>(message);

            return content.Message;
        }
    }

    internal class PrintNodeErrorMessage
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
