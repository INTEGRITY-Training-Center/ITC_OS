using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class CartController
    {
        public void InsertCart(CartInfo obj_cart)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_cart table_cart = (from a in db.tbl_carts
                                       where a.CustomerID == obj_cart.CustomerID && a.ProductID == obj_cart.ProductID
                                       select a).FirstOrDefault();
                if (table_cart != null)
                {
                    table_cart.Quantity = Convert.ToInt32(table_cart.Quantity) + obj_cart.Quantity;
                    db.SubmitChanges();
                }
                else
                {
                    tbl_cart table_Cart = new tbl_cart();
                    Guid id = Guid.NewGuid();
                    table_Cart.CartID = id.ToString();
                    table_Cart.ProductID = obj_cart.ProductID;
                    table_Cart.CustomerID = obj_cart.CustomerID;
                    table_Cart.Quantity = obj_cart.Quantity;

                    db.tbl_carts.InsertOnSubmit(table_Cart);
                    db.SubmitChanges();
                }                
            }
        }

        public void UpdateCart(CartInfo obj_cart)
        {
            using(OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_cart table_cart = (from a in db.tbl_carts where a.CartID == obj_cart.CartID select a).FirstOrDefault();
                if(table_cart != null)
                {
                    table_cart.Quantity = obj_cart.Quantity;
                    db.SubmitChanges();
                }
            }
        }

        public void DeleteCart(string cartID)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var table_cart = (from a in db.tbl_carts where a.CartID == cartID select a).FirstOrDefault();
                db.tbl_carts.DeleteOnSubmit(table_cart);
                db.SubmitChanges();
            }
        }

        public void DeleteAllCartByCustomerID(string customerID)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var table_carts = (from a in db.tbl_carts where a.CustomerID == customerID select a).ToList();
                foreach(var obj in table_carts)
                {
                    tbl_cart table_cart = (from a in db.tbl_carts where a.CartID == obj.CartID select a).FirstOrDefault();
                    db.tbl_carts.DeleteOnSubmit(table_cart);
                    db.SubmitChanges();
                }                
            }
        }

        public DataTable GetAllCartByCustomer(string customerID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CartID", typeof(string));
            dt.Columns.Add("ProductID", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("ProductImage", typeof(byte[]));
            dt.Columns.Add("Quantity", typeof(int)); 
            dt.Columns.Add("ProductPrice", typeof(string));//to change currency format
            dt.Columns.Add("TotalPrice", typeof(string));//to change currency format
            dt.Columns.Add("CustomerID", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("CustomerAddress", typeof(string));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllCartByCustomer(customerID);
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["CartID"] = obj.CartID;
                    dr["ProductID"] = obj.ProductID;
                    dr["ProductName"] = obj.ProductName;
                    dr["ProductImage"] = obj.ProductImage.ToArray();
                    dr["Quantity"] = obj.Quantity;
                    dr["ProductPrice"] = obj.ProductPrice;// String.Format("{0:n}", obj.ProductPrice);
                    dr["TotalPrice"] = obj.TotalPrice;// String.Format("{0:n}", obj.TotalPrice); 
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