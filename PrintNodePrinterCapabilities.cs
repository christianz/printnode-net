using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public class PrintNodePrinterCapabilities
    {
        public IEnumerable<string> Bins { get; set; } 
        public bool Collate { get; set; }
        public int Copies { get; set; }
        public bool Color { get; set; }
        public IEnumerable<string> Dpis { get; set; }
        public int[][] Extent { get; set; }
        public IEnumerable<string> Medias { get; set; } 
        public IEnumerable<int> Nup { get; set; } 
        public Dictionary<string, int[]> Papers { get; set; } 
        public Dictionary<string, string> PrintRate { get; set; } 

        [JsonProperty("supports_custom_paper_size")]
        public bool SupportsCustomPaperSize { get; set; }
    }
}
