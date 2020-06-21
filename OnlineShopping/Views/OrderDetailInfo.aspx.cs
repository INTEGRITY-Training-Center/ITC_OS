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
    public partial class OrderDetailInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (Request.QueryString["OrderID"].ToString() != null)
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
                
                string OrderID = Request.QueryString["OrderID"].ToString(); ;
                OrderController orderControl = new OrderController();
                OrderInfo obj_OrderInfo = orderControl.GetOrderByID(OrderID);
                //Bind to Header
                order_id.InnerText = OrderID;
                lblOrderNo.Text = obj_OrderInfo.OrderNo;
                lblOrderDate.Text = String.Format("{0:dd-MMM-yyyy}", obj_OrderInfo.OrderDate);
                lblCustomerName.Text = obj_OrderInfo.CustomerName;
                lblOrderQuantity.Text = obj_OrderInfo.OrderQuantity.ToString();
                lblCustomerAddress.Text = obj_OrderInfo.CustomerAddress;
                lblAdditionalRequest.Text = obj_OrderInfo.OrderDescription;
                //Bind to Total
                lblSubTotal.Text = obj_OrderInfo.OrderAmount.ToString();
                lblGrandTotal.Text = obj_OrderInfo.OrderAmount.ToString();

                OrderDetailController orderDetailControl = new OrderDetailController();
                DataTable dt_Cart = new DataTable();
                dt_Cart = orderDetailControl.GetAllOrderDetailByOrderID(OrderID);
                //Bind to List View
                productList.DataSource = dt_Cart;
                productList.DataBind();

                //already login and login user is admin
                if (!String.IsNullOrEmpty(CustomerID) && (!String.IsNullOrEmpty(IsAdmin) && Convert.ToBoolean(IsAdmin) == true))
                {
                    btnCheckOrder.Visible = true;
                    //Button Text change and disable/enable
                    if (obj_OrderInfo.OrderStatus == "Created")
                    {
                        btnCheckOrder.Enabled = true;
                        btnCheckOrder.Text = "Order Prepare";
                    }
                    else if (obj_OrderInfo.OrderStatus == "Preparing")
                    {
                        btnCheckOrder.Enabled = true;
                        btnCheckOrder.Text = "Order Deliver";
                    }
                    else if (obj_OrderInfo.OrderStatus == "Delivered")
                    {
                        btnCheckOrder.Enabled = false;
                        btnCheckOrder.Text = "Done";
                    }
                }
                else
                {
                    btnCheckOrder.Visible = false;
                }
            }
        }

        protected void btnCheckOrder_Click(object sender, EventArgs e)
        {
            string orderStatus = "";
            if(btnCheckOrder.Text == "Order Prepare")
            {
                orderStatus = "Preparing";
            }
            else if (btnCheckOrder.Text == "Order Deliver")
            {
                orderStatus = "Delivered";
            }

            OrderController orderControl = new OrderController();
            OrderInfo obj_orderInfo = new OrderInfo();
            obj_orderInfo.OrderID = order_id.InnerText;
            obj_orderInfo.OrderStatus = orderStatus;
            orderControl.UpdateOrderStatus(obj_orderInfo);

            Response.Redirect("OrderInfoList.aspx");
        }
    }
}