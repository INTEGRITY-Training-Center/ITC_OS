using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class WishlistController
    {
        public string InsertWishlist(Wishlist obj_wishlist)
        {
            string result = "";
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Wishlist table_wish = (from a in db.tbl_Wishlists
                                       where a.CustomerID == obj_wishlist.CustomerID && a.ProductID == obj_wishlist.ProductID
                                       select a).FirstOrDefault();
                if (table_wish != null)
                {
                    result = "You already added to wishlist.";
                }
                else
                {
                    tbl_Wishlist table_Wishlist = new tbl_Wishlist();
                    Guid id = Guid.NewGuid();
                    table_Wishlist.WishlistID = id.ToString();
                    table_Wishlist.ProductID = obj_wishlist.ProductID;
                    table_Wishlist.CustomerID = obj_wishlist.CustomerID;
                    table_Wishlist.AddedDate = obj_wishlist.AddedDate;

                    db.tbl_Wishlists.InsertOnSubmit(table_Wishlist);
                    db.SubmitChanges();

                    result = "Successfully added to the wishlist.";
                }
            }
            return result;
        }

        public void DeleteWishlist(string wilshlistID)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var table_wilshlist = (from a in db.tbl_Wishlists where a.WishlistID == wilshlistID select a).FirstOrDefault();
                db.tbl_Wishlists.DeleteOnSubmit(table_wilshlist);
                db.SubmitChanges();
            }
        }

        public Wishlist GetWishlistByID(string wishlistID)
        {
            Wishlist obj_Wishlist = new Wishlist();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Wishlist table_wishlist = (from a in db.tbl_Wishlists where a.WishlistID == wishlistID select a).FirstOrDefault();
                obj_Wishlist.WishlistID = table_wishlist.WishlistID;
                obj_Wishlist.ProductID = table_wishlist.ProductID;
                obj_Wishlist.CustomerID = table_wishlist.CustomerID;
                obj_Wishlist.AddedDate = table_wishlist.AddedDate;
            }

            return obj_Wishlist;
        }

        public DataTable GetAllWishlistByCustomer(string customerID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("WishlistID", typeof(string));
            dt.Columns.Add("ProductID", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("ProductImage", typeof(byte[]));
            dt.Columns.Add("ProductPrice", typeof(string));
            dt.Columns.Add("AddedDate", typeof(string));
            dt.Columns.Add("CustomerID", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("CustomerAddress", typeof(string));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllWishlistByCustomer(customerID);
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["WishlistID"] = obj.WishlistID;
                    dr["ProductID"] = obj.ProductID;
                    dr["ProductName"] = obj.ProductName;
                    dr["ProductImage"] = obj.ProductImage.ToArray();
                    dr["ProductPrice"] = obj.ProductPrice;
                    dr["AddedDate"] = String.Format("{0:dd-MMM-yyyy}", obj.AddedDate);
                    dr["CustomerID"] = obj.CustomerID;
                    dr["CustomerName"] = obj.CustomerName;
                    dr["CustomerAddress"] = obj.CustomerAddress;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
    }
}