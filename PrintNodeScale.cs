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
        /// The first element represents the mass returned by the scale in micrograms as a unsigned integer. If the scale is been 
        /// unable to calucate a weight null will be returned here. This most commonly happens when scales display a negative weight. 
        /// It seems to be a limitation of most scales USB implementations that, whilst they can usually display and return 
        /// negative weights on their physical interfaces, they do not send this information over usb. The second element represents 
        /// the resolution of the scale in micrograms where known or null if not known.Suppose a scale is displaying a weight of 
        /// 125g accurate to 5g the returned mass value array would be [125000000, 5000000].
        /// </summary>
        [JsonProperty("mass")]
        public int[] Mass { get; set; }

        /// <summary>
        /// A string identifier for the scales. By default this will be the scales manufacturer and description. The PrintNode 
        /// client may rename a scale. Max length 251 characters.
        /// </summary>
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        /// <summary>
        /// Zero indexed integer. A deviceName and deviceNum pair may be used to distingish between two identical devices. 
        /// That is to say, at any point in time a deviceName, deviceNum pair uniquely identifies a specific scale. The first
        /// scale connected to a client will recieve a deviceNum of 0. If a second, identical scale is connected before the 
        /// first is removed it will recieve a deviceNum of 1 and so on. A scales deviceNum won't change whilst a scale 
        /// is connected but a deviceNum be reused. For example, should a scale be disconnected and reconnected it will 
        /// recieve the lowest currently unused deviceNum.
        /// </summary>
        [JsonProperty("deviceNum")]
        public int DeviceNum { get; set; }

        /// <summary>
        /// A string description of the scales connection method. Eg, "USB1" or "COM0".
        /// </summary>
        [JsonProperty("port")]
        public string Port { get; set; }

        /// <summary>
        /// Support for counting scales. Should a scale support counting and make this information available over usb this value will be 
        /// returned here. null otherwise.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Scales usually allow display of mass in imperial or metric units. The keys in this object represent the units on dispay 
        /// at the scales physical interface at the time of measurement. At time of writing supported keys are 'g', 'kg', 'lb', 'oz' 
        /// by more may be added as we encounter them. The value for each key is a integer representation of the value in 1 millionths 
        /// (1/1000000ths) of that unit. This information is provided to allow for a accurate recreation of a scales physcial display 
        /// to be produced. You don't need to parse this into a SI unit as PrintNode has already done this for you, see the property 
        /// mass. E.g.a scales display values of "1.25 kg", "1200g" or "2lb 10.32oz" would have a the same mass value but measurement 
        /// value of {"kg":1250000}, {"g": 1200000000} and {"lb": 2000000, "oz": 10320000} respectively.
        /// </summary>
        [JsonProperty("measurement")]
        public PrintNodeScaleMeasurement Measurement { get; set; }

        /// <summary>
        /// The system time as reported by the client computer at the time of generation of the scales data. The accuracy of this 
        /// timestamp is entirely dependant on the system clock of the computer which generated the scales data which is entirely variable. 
        /// Innaccuracies of greater than ± 5 minutes are not uncommon. This has been been provided so that the total latency in the 
        /// system can be accurately measured if the consumer of the api is the same as the machine producing the scales data.
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
        public string NtpOffset { get; set; }

        /// <summary>
        /// The time the scales data has be stored at PrintNode before being delivered to an API client in milliseconds. For the streaming 
        /// and websocket api endpoints where data can be immediately delivered to subscribers this will be low - expect values around 
        /// ~3-10ms. For the polling HTTP GET endpoints this will normally be much larger. It is important to note that large values 
        /// here do not mean that the data in old.Clients update PrintNode when they connect, disconnect or the weight returned from 
        /// the scales changes.If the weight returned by the scale is stable the ageOfData field can grow but still be accurate.
        /// </summary>
        [JsonProperty("ageOfData")]
        public int AgeOfData { get; set; }

        /// <summary>
        /// The computer id.
        /// </summary>
        [JsonProperty("computerId")]
        public long ComputerId { get; set; }

        /// <summary>
        /// String description of the vendor or manufacturer of the scales device. This is supplied by the usb subsystem
        /// </summary>
        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        /// <summary>
        /// The device name
        /// </summary>
        [JsonProperty("product")]
        public string Product { get; set; }

        /// <summary>
        /// The USB device vendor id. See <a href="http://www.usb.org/developers/vendor/">here</a> for detailed description and here for up to date list of vendor and device ids.
        /// </summary>
        [JsonProperty("vendorId")]
        public int VendorId { get; set; }

        /// <summary>
        /// The USB device id. See <a href="http://www.usb.org/developers/vendor/">here</a> for detailed description and here for up to date list of vendor and device ids.
        /// </summary>
        [JsonProperty("deviceId")]
        public int DeviceId { get; set; }

        public static async Task<IEnumerable<PrintNodeScale>> ListForComputerAsync(long computerId, PrintNodeRequestOptions options = null)
        {
            var response = await ApiHelper.Get($"/computer/{computerId}/scales", options);

            return JsonConvert.DeserializeObject<List<PrintNodeScale>>(response);
        }

        public static async Task<PrintNodeScale> GetAsync(long computerId, string deviceName, PrintNodeRequestOptions options = null)
        {
            var response = await ApiHelper.Get($"/computer/{computerId}/scales/{deviceName}", options);

            return JsonConvert.DeserializeObject<PrintNodeScale>(response);
        }
    }
}
