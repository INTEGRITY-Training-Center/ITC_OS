using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class OrderInfo
    {
        public string OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime EstDeliveryDate { get; set; }
        public int OrderQuantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal OrderAmount { get; set; }
        public string OrderDescription { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
    }
}