using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;
using PrintNodeNet.Util;

namespace PrintNodeNet
{
    public sealed class PrintNodeComputer
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inet")]
        public string Inet { get; set; }

        [JsonProperty("inet6")]
        public string Inet6 { get; set; }

        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("jre")]
        public string Jre { get; set; }

        [JsonProperty("createTimeStamp")]
        public DateTime CreateTimeStamp { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public static async Task<IEnumerable<PrintNodeComputer>> ListAsync(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get("/computers", options);

            return JsonConvert.DeserializeObject<List<PrintNodeComputer>>(response);
        }

        public static async Task<PrintNodeComputer> GetAsync(long id, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computers/{id}", options);

            var list = JsonConvert.DeserializeObject<List<PrintNodeComputer>>(response);

            return list.FirstOrDefault();
        }

        /// <summary>
        /// Static method to retrieve a set of computers from the PrintNode API.
        /// </summary>
        /// <param name="computerSet">The list of ids of the computers.</param>
        /// <param name="options">The request options (allows API key modification)</param>
        /// <returns>The list of <seealso cref="PrintNodeComputer"/> matching to the given ids</returns>
        public static async Task<IEnumerable<PrintNodeComputer>> GetSetAsync(IEnumerable<long> computerSet, PrintNodeRequestOptions options = null)
        {
            string ids = new SetBuilder(computerSet).Build();

            var response = await PrintNodeApiHelper.Get($"/computers/{ids}", options);

            return JsonConvert.DeserializeObject<List<PrintNodeComputer>>(response);
        }

        public async Task<IEnumerable<PrintNodePrinter>> ListPrinters(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computers/{Id}/printers", options);

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

        }

        /// <summary>
        /// Static overwrite of the <seealso cref="ListPrinters(PrintNodeRequestOptions)"/> method
        /// to get the printers of the given computer.
        /// </summary>
        /// <param name="id">The id of the computer to get the printers from.</param>
        /// <param name="options">The request options (allows API key modification)</param>
        /// <returns>The list of <seealso cref="PrintNodePrinter"/> that belongs to the given computer</returns>
        public static async Task<IEnumerable<PrintNodePrinter>> ListPrinters(long id, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computers/{id}/printers", options);

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);

        }

        /// <summary>
        /// Static method to retrieve the list of <seealso cref="PrintNodePrinter"/> that belongs
        /// to the given set of computers.
        /// </summary>
        /// <param name="computerSet">The list of ids of the computers.</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>The list of <seealso cref="PrintNodePrinter"/> that belongs to the given set of computers.</returns>
        public static async Task<IEnumerable<PrintNodePrinter>> ListComputerSetPrintersAsync(IEnumerable<long> computerSet, PrintNodeRequestOptions options = null)
        {
            string ids = new SetBuilder(computerSet).Build();

            var response = await PrintNodeApiHelper.Get($"/computers/{ids}/printers", options);

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);
        }

        /// <summary>
        /// Static method to retrieve the given set of <seealso cref="PrintNodePrinter"/> if they
        /// belong to the given set of computers.
        /// </summary>
        /// <param name="computerSet">The list of ids of the computers.</param>
        /// <param name="printerSet">The list of ids of the printers.</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>The set of <seealso cref="PrintNodePrinter"/> that belongs to the given set of computers.</returns>
        public static async Task<IEnumerable<PrintNodePrinter>> ListComputerSetPrinterSetAsync(IEnumerable<long> computerSet, IEnumerable<long> printerSet, PrintNodeRequestOptions options = null)
        {
            string computerIds = new SetBuilder(computerSet).Build();
            string printerIds = new SetBuilder(printerSet).Build();

            var response = await PrintNodeApiHelper.Get($"/computers/{computerIds}/printers/{printerIds}", options);

            return JsonConvert.DeserializeObject<List<PrintNodePrinter>>(response);
        }
    }
}
