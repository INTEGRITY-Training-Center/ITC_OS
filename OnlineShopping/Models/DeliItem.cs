using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace OnlineShopping.Models
{
    public class DeliItem
    {
        public string DeliItemID { get; set; }
        public string OrderID { get; set; }
        public string DeliManID { get; set; }
        public Boolean Status { get; set; }
        public DateTime? EstDeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string OrderNo { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public int OrderQuantity { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string OrderDescription { get; set; }
        public string DeliMan_Name { get; set; }
        public string DeliMan_Mobile { get; set; }
    }
}