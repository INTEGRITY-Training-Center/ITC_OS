using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class ProductDiscountController
    {
        public Decimal GetDiscountByProductID(string productID)
        {
            decimal discountAmount = 0;
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetDiscountByProductID(productID);
                foreach (var obj in data)
                {
                    discountAmount = Convert.ToDecimal(obj.DiscountAmount);
                }
            }

            return discountAmount;
        }
    }
}