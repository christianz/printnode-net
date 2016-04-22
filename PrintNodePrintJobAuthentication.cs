using Newtonsoft.Json;

namespace PrintNode.Net
{
    public class PrintNodePrintJobAuthentication
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("credentials")]
        public PrintNodeCredentials Credentials { get; set; }
    }
}
