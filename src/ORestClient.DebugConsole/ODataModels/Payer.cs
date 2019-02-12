﻿using System.Collections.Generic;
using System.Runtime.Serialization;

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
        [DataMember(Name = "ToCustomer")]
        //[SQLite.Ignore]
        public List<Customer> Customers { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}