using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTownship { get; set; }
        public string CustomerPassword { get; set; }
        public Boolean? IsAdmin { get; set; }
    }
}