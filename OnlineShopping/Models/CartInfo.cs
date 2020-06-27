using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class CartInfo
    {
        public string CartID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public byte[] ProductImage { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string CustomerID { get; set; }

    }
}