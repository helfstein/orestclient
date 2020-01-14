using System.Collections.Generic;
using Newtonsoft.Json;
using ORest;

namespace ORestClient.Samples {
    public class VehicleOffers {

        [JsonConverter(typeof(ORestPropListConverter), "results")]
        public List<VehicleOfferItems> ToVehicleOfferItems {
            get;
            set;
        }

        public string OfferNumber {
            get;
            set;
        }

        public string Transporter {
            get;
            set;
        }

        public string TransporterName {
            get;
            set;
        }

        public string SalesOrg {
            get;
            set;
        }

        public string Plant {
            get;
            set;
        }

        public string PlantName {
            get;
            set;
        }
        [JsonConverter(typeof(ORestPropListConverter), "results")]
        public List<ResultMessages> ToResultMessages {
            get;
            set;
        }
        public ResultDetail ToResultDetail {
            get;
            set;
        }
    }
}
