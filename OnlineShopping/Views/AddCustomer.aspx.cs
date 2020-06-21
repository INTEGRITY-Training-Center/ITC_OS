﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class AddCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            CustomerController obj_Customer_control = new CustomerController();
            Customer obj_customer = new Customer();
            obj_customer.CustomerName = txtCustomerName.Text.Trim();
            obj_customer.CustomerEmail = txtEmail.Text.Trim();
            obj_customer.CustomerMobile = txtMobilePhone.Text.Trim();
            obj_customer.CustomerAddress = txtAddress.Text.Trim();
            obj_customer.CustomerPassword = txtPassword.Text;
            obj_customer.IsAdmin = false;

            obj_Customer_control.InsertCustomer(obj_customer);

            Response.Redirect("Login.aspx");
        }
    }
}