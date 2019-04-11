using Newtonsoft.Json;

namespace ORestClient.Samples {
    public class VehicleOfferItems {

        [JsonProperty("OfferNumber")]
        public string OfferNumber { get; set; }

        [JsonProperty("OfferItemNumber")]
        public string OfferItemNumber { get; set; }

        [JsonProperty("Plate")]
        public string Plate { get; set; }

        [JsonProperty("PlateName")]
        public string PlateName { get; set; }

        [JsonProperty("Coupled1")]
        public string Coupled1 { get; set; }

        [JsonProperty("Coupled1Name")]
        public string Coupled1Name { get; set; }

        [JsonProperty("Coupled2")]
        public string Coupled2 { get; set; }

        [JsonProperty("Coupled2Name")]
        public string Coupled2Name { get; set; }

        [JsonProperty("Driver")]
        public string Driver { get; set; }

        [JsonProperty("DriverName")]
        public string DriverName { get; set; }

        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("Availability")]
        public string Availability { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusDesc")]
        public string StatusDesc { get; set; }

        [JsonProperty("LoadType")]
        public string LoadType { get; set; }

        [JsonProperty("LoadTypeDesc")]
        public string LoadTypeDesc { get; set; }
    }
}
