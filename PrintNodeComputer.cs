using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrintNode.Net
{
    public sealed class PrintNodeComputer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Inet { get; set; }
        public string Inet6 { get; set; }
        public string HostName { get; set; }
        public string Jre { get; set; }
        public DateTime CreateTimeStamp { get; set; }
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
