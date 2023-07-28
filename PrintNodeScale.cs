using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrintNodeNet.Http;

namespace PrintNodeNet
{
    public sealed class PrintNodeScale
    {
        /// <summary>
        /// The first element represents the mass returned by the scale in micrograms. If the scale has been unable to calculate a
        /// weight, this element is null. This most commonly happens when scales display a negative weight. Although many scales can
        /// display negative weights on their built-in displays, they are often unable to return negative weight readings over their
        /// USB interfaces.
        ///
        /// The second element represents the resolution of the scale in micrograms, where this is known, or null otherwise.
        ///
        /// For example, a reading of 125g with a resolution of 5g would be represented as [125000000, 5000000].
        /// </summary>
        [JsonProperty("mass")]
        public int?[] Mass { get; set; }

        /// <summary>
        /// A string identifier for the scale. This is usually the scale's manufacturer and description, unless it has been renamed
        /// in the PrintNode Client.
        /// </summary>
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        /// <summary>
        /// If more than one scale with the same deviceName is connected to a computer, they will be assigned different deviceNum
        /// properties. This makes it possible to distinguish between them.
        ///
        /// deviceNum values start at 0 and when a scale is connected to a computer it is assigned the smallest unused deviceNum value
        /// for scales with the same deviceName.For example, if two scales with different deviceNames are connected they will both have
        /// deviceNum 0. If two scales with the same deviceName are connected they will be assigned deviceNums of 0 and 1. The scale
        /// which was connected first gets deviceNum 0.
        /// </summary>
        [JsonProperty("deviceNum")]
        public int DeviceNum { get; set; }

        /// <summary>
        /// A string representing the port to which the scale is connected, e.g. "USB1" or "COM0".
        /// </summary>
        [JsonProperty("port")]
        public string Port { get; set; }

        /// <summary>
        /// Reserved for future use. Currently null.
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; set; }

        /// <summary>
        /// Scales can usually display their readings in imperial or metric units. The keys in this object represent the units shown on the
        /// scale's built-in display at the time of measurement. Supported units are: g, kg, lb and oz. The value for each key is the reading
        /// in billionths of the corresponding unit. This information makes it easy to determine the reading being displayed on the scale
        /// itself. In terms of measurement, it provides the same information as the mass property. Use whichever one you find more convenient.
        ///
        /// For example, display values of "1.2 kg", "1200g" or "2lb 10.32oz" would result in measurement values of {"kg":1200000000},
        /// {"g": 1200000000000} and {"lb": 2000000000, "oz": 10320000000} respectively, but in all three cases the first element of the
        /// mass array would be 1200000000.
        /// </summary>
        [JsonProperty("measurement")]
        public PrintNodeScaleMeasurement Measurement { get; set; }

        /// <summary>
        /// The time as reported by the computer the scale is attached to at the time of generation of the scale data.
        /// </summary>
        [JsonProperty("clientReportedCreateTimeStamp")]
        public DateTime ClientReportedCreateTimeStamp { get; set; }

        /// <summary>
        /// Inaccuracy of the system time of the client computer generating the scales data in milliseconds as determined via by NTP. 
        /// Positive values represent a system clock running fast and a negative value represents a system clock running slow. Under normal 
        /// conditions this offset is usually accurate to ± 10 milliseconds. It takes a short while to determine the ntpOffset after the 
        /// client starts up. When this information is not available the value will be null.
        /// </summary>
        [JsonProperty("ntpOffset")]
        public int? NtpOffset { get; set; }

        /// <summary>
        /// Reserved for future use. Currently null.
        /// </summary>
        [JsonProperty("ageOfData")]
        public int AgeOfData { get; set; }

        /// <summary>
        /// The computer id.
        /// </summary>
        [JsonProperty("computerId")]
        public long ComputerId { get; set; }

        /// <summary>
        /// String description of the vendor or manufacturer of the scales device.
        /// </summary>
        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        /// <summary>
        /// The product name.
        /// </summary>
        [JsonProperty("product")]
        public string Product { get; set; }

        /// <summary>
        /// The USB device vendor id. See <a href="http://www.usb.org/developers/vendor/">here</a> for a detailed description
        /// and see <a href="http://www.linux-usb.org/usb.ids">here</a> for an up-to-date list of vendor and product ids.
        /// </summary>
        [JsonProperty("vendorId")]
        public int VendorId { get; set; }

        /// <summary>
        /// The USB device product id. See <a href="http://www.usb.org/developers/vendor/">here</a> for a detailed description and
        /// see <a href="http://www.linux-usb.org/usb.ids">here</a> for an up-to-date list of vendor and product ids.
        /// </summary>
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        public static async Task<IEnumerable<PrintNodeScale>> ListForComputerAsync(long computerId, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computer/{computerId}/scales", options);

            return JsonConvert.DeserializeObject<List<PrintNodeScale>>(response);
        }

        public static async Task<IEnumerable<PrintNodeScale>> ListForComputerDeviceAsync(long computerId, string deviceName, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computer/{computerId}/scales/{deviceName}", options);

            return JsonConvert.DeserializeObject<List<PrintNodeScale>>(response);
        }

        public static async Task<PrintNodeScale> GetAsync(long computerId, string deviceName, int deviceNum, PrintNodeRequestOptions options = null)
        {
            var response = await PrintNodeApiHelper.Get($"/computer/{computerId}/scale/{deviceName}/{deviceNum}", options);

            return JsonConvert.DeserializeObject<PrintNodeScale>(response);
        }
    }
}
