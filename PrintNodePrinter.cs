using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public sealed class PrintNodePrinter
    {
        public long Id { get; set; }
        public PrintNodeComputer Computer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PrintNodePrinterCapabilities Capabilities { get; set; }
        public string Default { get; set; }
        public DateTime CreateTimeStamp { get; set; }
        public string State { get; set; }

        public static async Task<IEnumerable<PrintNodePrinter>> ListAsync()
        {
            var response = await ApiHelper.Get("/printers");

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);
        }

        public static async Task<PrintNodePrinter> GetAsync(long id)
        {
            var response = await ApiHelper.Get("/printers/" + id);

            var list = JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

            return list.FirstOrDefault();
        }

        public async Task<long> AddPrintJob(PrintNodePrintJob job)
        {
            job.PrinterId = Id;

            var response = await ApiHelper.Post("/printjobs", job);

            return JsonConvert.DeserializeObject<long>(response);
        }
    }
}
