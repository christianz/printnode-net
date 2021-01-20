using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrintNodeNet
{
    public class PrintNodePrinterCapabilities
    {
        [JsonProperty("bins")]
        public IEnumerable<string> Bins { get; set; }

        [JsonProperty("collate")]
        public bool Collate { get; set; }

        [JsonProperty("copies")]
        public int Copies { get; set; }

        [JsonProperty("color")]
        public bool Color { get; set; }

        [JsonProperty("dpis")]
        public IEnumerable<string> Dpis { get; set; }

        [JsonProperty("extent")]
        public int[][] Extent { get; set; }

        [JsonProperty("medias")]
        public IEnumerable<string> Medias { get; set; }

        [JsonProperty("nup")]
        public IEnumerable<int> Nup { get; set; }

        [JsonProperty("papers")]
        public Dictionary<string, int?[]> Papers { get; set; }

        [JsonProperty("printRate")]
        public Dictionary<string, string> PrintRate { get; set; } 

        [JsonProperty("supports_custom_paper_size")]
        public bool SupportsCustomPaperSize { get; set; }
    }
}
