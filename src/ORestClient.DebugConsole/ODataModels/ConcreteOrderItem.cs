﻿namespace ORestClient.Samples.ODataModels {
    //=============================================================================================
    public class ConcreteOrderItem {

        #region Properties
        //-----------------------------------------------------------------------------------------
        public string OrderID { get; set; }
        //-----------------------------------------------------------------------------------------
        public string ItemID { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Material { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Description { get; set; }
        //-----------------------------------------------------------------------------------------
        public decimal OrderQuantity { get; set; }
        //-----------------------------------------------------------------------------------------
        public decimal DeliveredQuantity { get; set; }
        //-----------------------------------------------------------------------------------------
        public string SalesUnit { get; set; }
        //-----------------------------------------------------------------------------------------
        public string MaterialPricingGroup { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Element { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}