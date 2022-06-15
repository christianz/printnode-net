using Newtonsoft.Json;

namespace PrintNodeNet
{
    public class PrintNodeScaleMeasurement
    {
        /// <summary>
        /// The measurement in kilos
        /// </summary>
        [JsonProperty("kg")]
        public long Kg { get; set; }

        /// <summary>
        /// The measurement in grams
        /// </summary>
        [JsonProperty("g")]
        public long G { get; set; }

        /// <summary>
        /// The measurement in pounds
        /// </summary>
        [JsonProperty("lb")]
        public long Lb { get; set; }

        /// <summary>
        /// The measurement in ounces
        /// </summary>
        [JsonProperty("oz")]
        public long Oz { get; set; }
    }
}