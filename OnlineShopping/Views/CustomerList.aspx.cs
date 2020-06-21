using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;

namespace OnlineShopping.Views
{
    public partial class CustomerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerController customer_control = new CustomerController();
            gvCustomerList.DataSource = customer_control.GetAllCustomer();
            gvCustomerList.DataBind();
        }
    }
}