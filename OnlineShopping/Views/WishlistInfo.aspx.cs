using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class WishlistInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["CustomerID"] != null)
            {
                string customerID = Request.Cookies["CustomerID"].Value;

                if (!IsPostBack)
                {
                    if (!String.IsNullOrEmpty(customerID))
                    {
                        DataBindToPage(customerID);
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        public void DataBindToPage(string customerID)
        {
            WishlistController wishlistController = new WishlistController();
            DataTable dt_Wishlist = new DataTable();
            dt_Wishlist = wishlistController.GetAllWishlistByCustomer(customerID);

            //Bind to List View
            productList.DataSource = dt_Wishlist;
            productList.DataBind();
           
            customer_id.InnerText = customerID;
        }

        protected void productList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var wishlistID = e.CommandArgument;
            WishlistController wishlistControl = new WishlistController();
            if (e.CommandName == "Delete")
            {
                wishlistControl.DeleteWishlist(wishlistID.ToString());

                DataBindToPage(customer_id.InnerText);
            }
            else if(e.CommandName == "InsertData")
            {
                Wishlist obj_wishlist = new Wishlist();
                obj_wishlist = wishlistControl.GetWishlistByID(wishlistID.ToString());
                //Add to Cart
                CartController obj_cart_control = new CartController();
                CartInfo obj_cart = new CartInfo();
                obj_cart.ProductID = obj_wishlist.ProductID;
                obj_cart.CustomerID = obj_wishlist.CustomerID;
                obj_cart.Quantity = Convert.ToInt32("1");
                obj_cart_control.InsertCart(obj_cart);
                //Delete Wishlist
                wishlistControl.DeleteWishlist(wishlistID.ToString());

                DataBindToPage(customer_id.InnerText);
            }
        }

        protected void productList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }
        
        protected void productList_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }
    }
}