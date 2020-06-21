using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;

namespace OnlineShopping.Views
{
    public partial class OrderInfoList : System.Web.UI.Page
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

            //already login and login user is admin
            if (!String.IsNullOrEmpty(CustomerID) && (!String.IsNullOrEmpty(IsAdmin) && Convert.ToBoolean(IsAdmin) == true))
            {
                OrderController order_control = new OrderController();
                gvOrderInfo.DataSource = order_control.GetAllOrderInfo();
                gvOrderInfo.DataBind();
            }
            //already login but login user is not admin
            else if (!String.IsNullOrEmpty(CustomerID) && (!String.IsNullOrEmpty(IsAdmin) && Convert.ToBoolean(IsAdmin) == false))
            {
                OrderController order_control = new OrderController();
                gvOrderInfo.DataSource = order_control.GetAllOrderByCustomerID(CustomerID);
                gvOrderInfo.DataBind();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}