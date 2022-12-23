using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;
using PrintNodeNet.Util;

namespace PrintNodeNet
{
    public sealed class PrintNodePrintJob
    {
        /// <summary>
        /// Assigned by API. Any value submitted here will be ignored.
        /// </summary>
        [JsonProperty("id")]
        public long? Id { get; set; }

        /// <summary>
        /// The printer id associated with your account.
        /// </summary>
        [JsonProperty("printerId")]
        public long PrinterId { get; set; }

        [JsonProperty("printer")]
        public PrintNodePrinter Printer { get; set; }

        /// <summary>
        /// A title to give the PrintJob. This is the name which will appear in the operating system's print queue.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Either 'pdf_uri', 'pdf_base64', 'raw_uri', 'raw_base64'. 
        /// 
        /// See <a href="https://www.printnode.com/docs/api/curl/#create-printjob-content">content</a>.
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// A uri accessible by the client when contentType is 'pdf_uri'.
        /// or
        /// A base64 encoded representation of the pdf when contentType is 'pdf_base64'.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// A text description of how the printjob was created or where the printjob originated.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; } = "printnode-net";

        /// <summary>
        /// An object describing various options which can be set for this PrintJob. See options. Printing options have no effect when raw printing.
        /// </summary>
        [JsonProperty("options")]
        public PrintNodePrintJobOptions Options { get; set; }

        /// <summary>
        /// The maximum number of seconds PrintNode should retain this PrintJob for attempted printing in the event the PrintJob cannot be 
        /// printed immediately. The current default is 14 days or 1,209,600 seconds.
        /// </summary>
        [JsonProperty("expireAfter")]
        public long? ExpireAfter { get; set; }

        /// <summary>
        /// The default value is 1. A positive integer representing the number of times this PrintJob should be delivered to the print queue. 
        /// This differs from the "copies" option in that this will send a document to a printer multiple times and does not rely on print driver 
        /// support. This is the only way to support multiple copies when raw printing. This also enables printing multiple copies even when a 
        /// printer driver does not natively support this.
        /// </summary>
        [JsonProperty("qty")]
        public int Qty { get; set; } = 1;

        /// <summary>
        /// If a contentType of 'pdf_uri' or 'raw_uri' is used and the uri requires either HTTP Basic or Digest Authentication you can specify 
        /// the username and password information here. Supported in clients v4.7.0 or newer.
        /// 
        /// For Basic authentication
        /// {
        ///     "type": "BasicAuth",
        ///     "credentials": {
        ///         "user": "username",
        ///         "pass": "password"
        ///     }
        /// }
        /// 
        /// For Digest authentication
        /// 
        /// {
        ///     "type": "DigestAuth",
        ///     "credentials": {
        ///         "user": "username",
        ///         "pass": "password"
        ///     }
        /// }
        /// Just replace the "username" and "password" with your credentials as appropriate.
        /// </summary>
        [JsonProperty("authentication")]
        public PrintNodePrintJobAuthentication Authentication { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("createTimeStamp")]
        public DateTime? CreateTimeStamp { get; set; }

		public PrintNodePrintJob() { }

		public PrintNodePrintJob(string title, PrintNodeContentType contentType, string content, long printerId)
		{
			Title = title;
			ContentType = contentType.ToString();
			Content = content;
			PrinterId = printerId;
		}


        public static async Task<IEnumerable<PrintNodePrintJob>> ListAsync(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get("/printjobs", options);

            return JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);
        }

        public static async Task<IEnumerable<PrintNodePrintJob>> ListForPrinterAsync(long printerId, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/printers/{printerId}/printjobs", options);

            return JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);
        }

        /// <summary>
        /// Static method that allows the retrieval of the list of <seealso cref="PrintNodePrintJob"/>
        /// for a set of <seealso cref="PrintNodePrinter"/>.
        /// </summary>
        /// <param name="set">The ids of the printers</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>The print jobs of every printer that has its ID included in the set</returns>
        public static async Task<IEnumerable<PrintNodePrintJob>> ListForPrinterSetAsync(IEnumerable<long> set, PrintNodeRequestOptions options = null)
        {
            string ids = new SetBuilder(set).Build();

            var response = await PrintNodeApiHelper.Get($"/printers/{ids}/printjobs", options);

            return JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);
        }

        /// <summary>
        /// Static method that allows the retrieval of a set of <seealso cref="PrintNodePrintJob"/>
        /// for a set of <seealso cref="PrintNodePrinter"/>.
        /// </summary>
        /// <param name="printerSet">The ids of the printers</param>
        /// <param name="printjobSet">The ids of the print jobs</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>The union between print jobs of the given ids and printjobs of the given printers.</returns>
        public static async Task<IEnumerable<PrintNodePrintJob>> ListPrinjobSetForPrinterSetAsync(IEnumerable<long> printerSet, IEnumerable<long> printjobSet, PrintNodeRequestOptions options = null)
        {
            string printerIds = new SetBuilder(printerSet).Build();
            string printjobsIds = new SetBuilder(printjobSet).Build();

            var response = await PrintNodeApiHelper.Get($"/printers/{printerIds}/printjobs/{printjobsIds}", options);

            return JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);
        }

        public static async Task<PrintNodePrintJob> GetAsync(long id, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/printjobs/{id}", options);

            var list = JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);

            return list.FirstOrDefault();
        }

        /// <summary>
        /// Static method that allows the retrieval of a set of <seealso cref="PrintNodePrintJob"/>,
        /// identified by their IDs.
        /// </summary>
        /// <param name="set">The ids of the print jobs</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>The print jobs as an enumerable.</returns>
        public static async Task<IEnumerable<PrintNodePrintJob>> GetSetAsync(IEnumerable<long> set, PrintNodeRequestOptions options = null)
        {
            string ids = new SetBuilder(set).Build();

            var response = await PrintNodeApiHelper.Get($"/printjobs/{ids}", options);

            return JsonConvert.DeserializeObject<IEnumerable<PrintNodePrintJob>>(response);
        }

        public async Task<IEnumerable<PrintNodePrintJobState>> GetStates(PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/printjobs/{Id}/states", options);

            var list = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PrintNodePrintJobState>>>(response);

            return list.FirstOrDefault();
        }

        /// <summary>
        /// Static overwrite of the <seealso cref="GetStates(PrintNodeRequestOptions)"/> method.
        /// It retrieves all the history of states for the given printjob id.
        /// </summary>
        /// <param name="id">The print job ID to get the history from.</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>A list of <seealso cref="PrintNodePrintJobState"/> that traces every state the printjob has been in since its creation.</returns>
        public static async Task<IEnumerable<PrintNodePrintJobState>> GetStates(long id, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/printjobs/{id}/states", options);

            var list = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PrintNodePrintJobState>>>(response);

            return list.FirstOrDefault();
        }

        /// <summary>
        /// This method retrieves all the history of states for the given set of print jobs.
        /// </summary>
        /// <param name="set">The ids of all the print jobs we want to get the states of.</param>
        /// <param name="options">The request options (allows API key modification).</param>
        /// <returns>A list of <seealso cref="PrintNodePrintJobState"/> that traces every state the print job has been in since its creation, for every print job of the set.</returns>
        public static async Task<IEnumerable<IEnumerable<PrintNodePrintJobState>>> GetPrintJobSetStatesAsync(IEnumerable<long> set, PrintNodeRequestOptions options = null)
        {
            string ids = new SetBuilder(set).Build();

            var response = await PrintNodeApiHelper.Get($"/printjobs/{ids}/states", options);

            return JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PrintNodePrintJobState>>>(response);
        }

		 public async Task<long> Print(PrintNodeRequestOptions options = null)
        {
            if (Printer == null && PrinterId == 0)
			{
				throw new Exception("Printer or PrinterId required");
			}

            var response = await PrintNodeApiHelper.Post("/printjobs", this, options);

            return JsonConvert.DeserializeObject<long>(response);
        }
    }
}
