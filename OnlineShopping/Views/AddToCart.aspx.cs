using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class AddToCart : System.Web.UI.Page
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
            CartController cartControl = new CartController();
            DataTable dt_Cart = new DataTable();
            dt_Cart = cartControl.GetAllCartByCustomer(customerID);
            //Bind to List View
            productList.DataSource = dt_Cart;
            productList.DataBind();
            //Bind to Total
            decimal d_total = 0;
            foreach (DataRow dr in dt_Cart.Rows)
            {
                d_total += Convert.ToDecimal(dr["TotalPrice"]);
            }

            if(dt_Cart.Rows.Count <= 0)
            {
                btnCreateOrder.Enabled = false;
            }
            else
            {
                btnCreateOrder.Enabled = true;
            }
            lblSubTotal.Text = d_total.ToString();// String.Format("{0:n}", d_total);  // Output: 1,234.00
            lblGrandTotal.Text = d_total.ToString();// String.Format("{0:n}", d_total);
            customer_id.InnerText = customerID;
        }

        [WebMethod]
        public static string UpdateCart(string cart_id, int currentQty)
        {
            string result = "";
            CartController obj_cart_control = new CartController();
            CartInfo obj_cart = new CartInfo();
            obj_cart.CartID = cart_id;
            obj_cart.Quantity = currentQty;
            obj_cart_control.UpdateCart(obj_cart);
            result = "Success";
            
            return result;
        }

        protected void productList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var cartID = e.CommandArgument;

            if (e.CommandName == "Delete")
            {
                CartController cartControl = new CartController();
                cartControl.DeleteCart(cartID.ToString());

                DataBindToPage(customer_id.InnerText);
            }
        }
        //Need to create because of "productList_ItemCommand" event
        protected void productList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            int orderQty = 0;
            string customerID = customer_id.InnerText;

            //============ Data Preparation ============
            List<OrderDetail> lst_OrderDetailInfo = new List<OrderDetail>();

            CartController cartControl = new CartController();
            DataTable dt_Cart = new DataTable();
            dt_Cart = cartControl.GetAllCartByCustomer(customerID);
            foreach(DataRow dr in dt_Cart.Rows)
            {
                orderQty += Convert.ToInt32(dr["Quantity"]);
                OrderDetail obj_OrderDetail = new OrderDetail();
                obj_OrderDetail.ProductID = dr["ProductID"].ToString();
                obj_OrderDetail.Quantity = Convert.ToInt32(dr["Quantity"]);
                obj_OrderDetail.Price = Convert.ToDecimal(dr["ProductPrice"]);
                lst_OrderDetailInfo.Add(obj_OrderDetail);
            }

            OrderInfo obj_OrderInfo = new OrderInfo();
            obj_OrderInfo.OrderDate = DateTime.Now;
            obj_OrderInfo.OrderQuantity = orderQty;
            obj_OrderInfo.OrderAmount = Convert.ToDecimal(lblGrandTotal.Text);
            obj_OrderInfo.OrderDescription = txtOrderDescription.Text;
            obj_OrderInfo.OrderStatus = "Created";
            obj_OrderInfo.CustomerID = customerID;

            //============ Insert Data ============
            OrderController orderControl = new OrderController();
            OrderInfo return_OrderInfo = orderControl.InsertOrder(obj_OrderInfo);
            if(return_OrderInfo != null)
            {
                OrderDetailController orderDetailControl = new OrderDetailController();
                orderDetailControl.InsertOrderDetail(return_OrderInfo.OrderID, lst_OrderDetailInfo);

                //============ Delete Data ============
                cartControl.DeleteAllCartByCustomerID(customerID);
                File.Delete(Server.MapPath(customerID + "_cart.json"));
                ExportOrderVoucher(return_OrderInfo.OrderID, return_OrderInfo.OrderNo);
            }

            Response.Redirect("Index.aspx");
            #region +++ When you want to get data from Listview +++
            ////for (int i = 0; i < productList.Items.Count; i++)
            ////{
            ////    Label lblProductID = (Label)productList.Items[i].FindControl("lbl_ProductID");
            ////    string productID = lblProductID.Text;
            ////}
            #endregion

        }

        private void ExportOrderVoucher(string OrderID, string OrderNo)
        {
            #region+++ Report Create ++++
            OrderController order_controller = new OrderController();
            OrderInfo obj_OrderInfo = order_controller.GetOrderByID(OrderID);
            //Header Parameters
            ReportParameter rp_orderno = new ReportParameter("p_orderno", obj_OrderInfo.OrderNo);
            ReportParameter rp_order_date = new ReportParameter("p_order_date", String.Format("{0:dd-MMM-yyyy}", obj_OrderInfo.OrderDate));
            ReportParameter rp_order_quantity = new ReportParameter("p_order_quantity", obj_OrderInfo.OrderQuantity.ToString());
            ReportParameter rp_customer_name = new ReportParameter("p_customer_name", obj_OrderInfo.CustomerName);
            ReportParameter rp_customer_address = new ReportParameter("p_customer_address", obj_OrderInfo.CustomerAddress);
            ReportParameter rp_additional_request = new ReportParameter("p_additional_request", obj_OrderInfo.OrderDescription);
            //Total Parameters
            ReportParameter rp_sub_total = new ReportParameter("p_sub_total", String.Format("{0:##,###.00}", obj_OrderInfo.OrderAmount.ToString()));
            ReportParameter rp_grand_total = new ReportParameter("p_grand_total", obj_OrderInfo.OrderAmount.ToString());
            ReportParameter rp_tax = new ReportParameter("p_tax", "0.00");
            //Report DataSet
            OrderDetailController order_Detail_controller = new OrderDetailController();
            ReportDataSource rds = new ReportDataSource("DataSet1", order_Detail_controller.GetAllOrderDetailByOrderID(OrderID));

            ReportViewer rvOrderVoucher = new ReportViewer();
            rvOrderVoucher.LocalReport.Refresh();
            rvOrderVoucher.LocalReport.DataSources.Clear();
            rvOrderVoucher.LocalReport.ReportPath = Server.MapPath("RDLC_OrderVoucher.rdlc");
            rvOrderVoucher.LocalReport.SetParameters(new ReportParameter[] { rp_orderno, rp_order_date, rp_order_quantity, rp_customer_name, rp_customer_address, rp_additional_request, rp_sub_total, rp_grand_total, rp_tax });
            rvOrderVoucher.LocalReport.DataSources.Add(rds);
            byte[] bytes = rvOrderVoucher.LocalReport.Render("PDF");
            #endregion

            MemoryStream memoryStream = new MemoryStream(bytes);
            memoryStream.Seek(0, SeekOrigin.Begin);

            MailMessage message = new MailMessage();
            message.Subject = "Order Voucher";
            message.IsBodyHtml = true;
            message.From = new MailAddress("dr.mail.mm@gmail.com");
            message.To.Add(obj_OrderInfo.CustomerEmail);
            //message.CC.Add("santosh.poojari@gmail.com");
            Attachment attachment = new Attachment(memoryStream, OrderNo + ".pdf");
            message.Attachments.Add(attachment);
            
            message.Body = @"Dear Customer, \n \t This is your order voucher. Please see in attached file!";

            NetworkCredential cred = new NetworkCredential("dr.mail.mm@gmail.com", "DDrrmm11@@");

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = cred;
            smtp.Port = 587;
            smtp.Send(message);

            memoryStream.Close();
            memoryStream.Dispose();
        }
    }
}