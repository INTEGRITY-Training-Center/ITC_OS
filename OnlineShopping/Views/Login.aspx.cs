using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HtmlAnchor link = (HtmlAnchor)(this.Master).FindControl("la");
            //string login_state = link.InnerText;
            txtCusName.Focus();
            if (Request.Cookies["CustomerID"] != null)
            {
                string s = Request.Cookies["CustomerID"].Value;

                if (!IsPostBack)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        Response.Cookies["CustomerID"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["IsAdmin"].Expires = DateTime.Now.AddDays(-1);

                        Response.Redirect("Index.aspx");
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string name = txtCusName.Text.Trim();
            string password = txtPassword.Text.Trim();
            CustomerController customerControl = new CustomerController();
            Customer obj_customer = new Customer();

            obj_customer = customerControl.CheckCustomerLogin(name, password);
            if (!String.IsNullOrEmpty(obj_customer.CustomerID))
            {
                Response.Cookies["CustomerID"].Value = obj_customer.CustomerID;
                Response.Cookies["CustomerID"].Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies["IsAdmin"].Value = obj_customer.IsAdmin.ToString();
                Response.Cookies["IsAdmin"].Expires = DateTime.Now.AddMinutes(30);

                Response.Redirect("Index.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock
                    (this, this.GetType(), "alertMessage", "alert('Name or Password is wrong. Please try again!')", true);
            }
        }
    }
}