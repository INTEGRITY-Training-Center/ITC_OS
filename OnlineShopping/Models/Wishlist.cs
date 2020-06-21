using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopping.Models
{
    public class Wishlist
    {
        public string WishlistID { get; set; }
        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public DateTime AddedDate { get; set; }
    }
}