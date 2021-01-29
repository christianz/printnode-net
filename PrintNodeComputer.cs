using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;

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
    }
}
