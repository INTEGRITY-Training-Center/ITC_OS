using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class ProductDiscount
    {
        public string DiscountID { get; set; }
        public string ProductID { get; set; }
        public string DiscountTypeID { get; set; }
        public decimal Discount { get; set; }
    }
}