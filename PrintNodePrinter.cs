using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;

namespace PrintNodeNet
{
    public sealed class PrintNodePrinter
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("computer")]
        public PrintNodeComputer Computer { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("capabilities")]
        public PrintNodePrinterCapabilities Capabilities { get; set; }

        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("createTimeStamp")]
        public DateTime CreateTimeStamp { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public static async Task<IEnumerable<PrintNodePrinter>> ListAsync(PrintNodeRequestOptions options = null)
        {
            var response = await ApiHelper.Get("/printers", options);

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);
        }

        public static async Task<PrintNodePrinter> GetAsync(long id, PrintNodeRequestOptions options = null)
        {
            var response = await ApiHelper.Get($"/printers/{id}", options);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

            return list.FirstOrDefault();
        }

        public async Task<long> AddPrintJob(PrintNodePrintJob job, PrintNodeRequestOptions options = null)
        {
            job.PrinterId = Id;

            var response = await ApiHelper.Post("/printjobs", job, options);

            return JsonConvert.DeserializeObject<long>(response);
        }
    }
}
