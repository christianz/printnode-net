using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
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

        public static async Task<IEnumerable<PrintNodeComputer>> ListAsync()
        {
            var response = await ApiHelper.Get("/computers");

            return JsonConvert.DeserializeObject<List<PrintNodeComputer>>(response);
        }

        public static async Task<PrintNodeComputer> GetAsync(long id)
        {
            var response = await ApiHelper.Get("/computers/" + id);

            var list = JsonConvert.DeserializeObject<List<PrintNodeComputer>>(response);

            return list.FirstOrDefault();
        }
    }
}
