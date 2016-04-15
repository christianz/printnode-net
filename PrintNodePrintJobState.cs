using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public sealed class PrintNodePrintJobState
    {
        /// <summary>
        /// The id of the PrintJob this state relates to.
        /// </summary>
        public long PrintJobId { get; set; }

        /// <summary>
        /// The PrintJob string. The allowed states are pending_confirmation, new, sent_to_client, deleted, 
        /// done, error, in_progress, queued, disappeared, received, downloading, downloaded, 
        /// preparing_to_print, queued_to_print and expired.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Additional human readable information about the state. If a error has occoured this where to look 
        /// to diagnose the problem.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A machine readable representation of the information contained in the property 'message'. Implmented 
        /// shortly. Currently this is null.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The version of the client which reported this state. This will be null null if the state did 
        /// not originate from the client.
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// The UTC date this state was reported to the server.
        /// </summary>
        public DateTime CreateTimeStamp { get; set; }

        /// <summary>
        /// The age of this state relative to the first state in milliseconds.
        /// </summary>
        public int Age { get; set; }

        public static async Task<IEnumerable<IEnumerable<PrintNodePrintJobState>>> ListAsync()
        {
            var response = await ApiHelper.Get("/printjobs/states");

            return JsonConvert.DeserializeObject<IEnumerable<IEnumerable<PrintNodePrintJobState>>>(response);
        }
    }
}
