using Newtonsoft.Json;
using PrintNodeNet;

namespace PrintNodeNet
{
    public class PrintNodePrintJobAuthentication
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("credentials")]
        public PrintNodeCredentials Credentials { get; set; }
    }
}
