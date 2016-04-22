using Newtonsoft.Json;

namespace PrintNode.Net
{
    public class PrintNodeCredentials
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }
    }
}
