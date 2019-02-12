using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ORest.Models;

namespace ORestClient.Samples.ODataModels {
    //=============================================================================================
    public class ConcreteOrder  {

        #region Constructor
        //-----------------------------------------------------------------------------------------
        public ConcreteOrder() {
            Deliveries = new List<ConcreteOrderDelivery>();
            Items = new List<ConcreteOrderItem>();
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Properties
        //-----------------------------------------------------------------------------------------
        public string OrderID { get; set; }
        //-----------------------------------------------------------------------------------------
        public string SalesOrganization { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Customer { get; set; }
        //-----------------------------------------------------------------------------------------
        public string CustomerName { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Site { get; set; }
        //-----------------------------------------------------------------------------------------
        public string SiteName { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Address { get; set; }
        //-----------------------------------------------------------------------------------------
        public string AddressName { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Status { get; set; }
        //-----------------------------------------------------------------------------------------
        public string StatusDescription { get; set; }
        //-----------------------------------------------------------------------------------------
        public DateTime StartDate { get; set; }
        //-----------------------------------------------------------------------------------------
        public DateTime? ValidTo { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Comment { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Interval { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Material { get; set; }
        //-----------------------------------------------------------------------------------------
        public string MaterialDescription { get; set; }
        //-----------------------------------------------------------------------------------------
        public string Element { get; set; }
        //-----------------------------------------------------------------------------------------
        public decimal Quantity { get; set; }
        //-----------------------------------------------------------------------------------------
        //TODO: Change this property when sap gateway updates
        private decimal _deliveredQuantity;
        public decimal DeliveredQuantity {
            get  {
                if (PendingQuantity > 0) {
                    _deliveredQuantity = Quantity - PendingQuantity;
                }
                return _deliveredQuantity;
            }
            set => _deliveredQuantity = value;
        }
        //-----------------------------------------------------------------------------------------
        public decimal PendingQuantity { get; set; }
        //-----------------------------------------------------------------------------------------
        public string UnloadingType { get; set; }
        //-----------------------------------------------------------------------------------------
        public string AssignedPump { get; set; }
        //-----------------------------------------------------------------------------------------
        //[SQLite.Ignore]
        public List<ConcreteOrderDelivery> Deliveries { get; set; }
        //-----------------------------------------------------------------------------------------
        //[SQLite.Ignore]
        //[JsonProperty("ToDeliveries")]
        public EntityContent<List<ConcreteOrderDelivery>> ToDeliveries {
            get => new EntityContent<List<ConcreteOrderDelivery>> {
                Results = Deliveries
            };
            set {
                if(value != null && value.Results.Any()) {
                    Deliveries = value.Results;
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        //[SQLite.Ignore]
        public List<ConcreteOrderItem> Items { get; set; }
        //-----------------------------------------------------------------------------------------
       // [SQLite.Ignore]
        //[JsonProperty("ToItems")]
        public EntityContent<List<ConcreteOrderItem>> ToItems {
            get => new EntityContent<List<ConcreteOrderItem>> {
                Results = Items
            };
            set {
                if(value != null && value.Results.Any()) {
                    Items = value.Results;
                }
            }
        }
        //-----------------------------------------------------------------------------------------
        #endregion

        #region Properties Aux
        //-----------------------------------------------------------------------------------------
        public string Payer { get; set; }
        //-----------------------------------------------------------------------------------------
        //[SQLite.Ignore]
        //public Color StatusColor { get; set; }
        //-----------------------------------------------------------------------------------------
        //[SQLite.Ignore]
        //public Color StatusTextColor { get; set; }
        //-----------------------------------------------------------------------------------------
        #endregion

    }
    //=============================================================================================
}