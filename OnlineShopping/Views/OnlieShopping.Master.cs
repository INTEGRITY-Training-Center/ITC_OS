using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineShopping.Views
{
    public partial class OnlieShopping : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string CustomerID = "", IsAdmin = "";
            if (Request.Cookies["CustomerID"] != null)
            {
                CustomerID = Request.Cookies["CustomerID"].Value;
            }
            if (Request.Cookies["IsAdmin"] != null)
            {
                IsAdmin = Request.Cookies["IsAdmin"].Value;
            }

            if(!String.IsNullOrEmpty(CustomerID))
            {
                la.InnerHtml = "Logout";
            }   
            else
            {
                la.InnerHtml = "Login";
            }

            //already login and login user is admin
            if (!String.IsNullOrEmpty(CustomerID) && (!String.IsNullOrEmpty(IsAdmin) && Convert.ToBoolean(IsAdmin) == true))
            {
                productLink.Visible = true;
                orderLink.Visible = true;
                customerLink.Visible = true;
                wishlistLink.Visible = false;
                myOrderLink.Visible = false;
            }
            else
            {
                productLink.Visible = false;
                orderLink.Visible = false;
                customerLink.Visible = false;
                wishlistLink.Visible = true;
                myOrderLink.Visible = true;
            }
           
        }
    }
}