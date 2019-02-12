using System.Collections.Generic;
using Newtonsoft.Json;
using ORest;

namespace ORestClient.Samples.ODataModels {
    //=============================================================================================
    public class Payer  {

        #region Constructor
        //-----------------------------------------------------------------------------------------
        public Payer() {
            Customers = new List<Customer>();
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Properties
        //-----------------------------------------------------------------------------------------
        public string Partner { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Name { get; set; }
        //-----------------------------------------------------------------------------------------
        //[JsonProperty("ToCustomer")]
        [JsonProperty("ToCustomer")]
        [JsonConverter(typeof(ORestPropListConverter), "results")]
        //[SQLite.Ignore]
        public List<Customer> Customers { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion
        
    }
    //=============================================================================================
}