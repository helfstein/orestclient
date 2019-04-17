using System;
using Newtonsoft.Json;
using ORest;

namespace ORestClient.Samples {
    public class Travels {
        public string TransportNumber { get; set; }
        public string Supplier { get; set; }
        public string SupplierName { get; set; }
        public string Plant { get; set; }
        public string PlantName { get; set; }
        public string Plate { get; set; }
        public string PlateName { get; set; }
        public string MaterialDesc { get; set; }
    
        public DateTime Programmed { get; set; }
        public float QuantityDeliveries { get; set; }
        public string TripType { get; set; }
        public bool NightDriving { get; set; }
        public string Type { get; set; }
        public string Destination { get; set; }
    }
}