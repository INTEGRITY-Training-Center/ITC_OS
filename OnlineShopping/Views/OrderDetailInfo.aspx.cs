using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
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
                        btnCheckOrder.Text = "Order Confirm";
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
            OrderController orderControl = new OrderController();
            OrderInfo obj_orderInfo = new OrderInfo();
            obj_orderInfo.OrderID = order_id.InnerText;
            obj_orderInfo.OrderStatus = "Confirmed";
            orderControl.UpdateOrderStatus(obj_orderInfo);

            //Send Voucher to Customer
            ExportOrderVoucher(order_id.InnerText);

            Response.Redirect("OrderInfoList.aspx");
        }

        private void ExportOrderVoucher(string OrderID)
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
            Attachment attachment = new Attachment(memoryStream, obj_OrderInfo.OrderNo + ".pdf");
            message.Attachments.Add(attachment);
            message.Body = String.Format("Dear {0},<p>This is your order voucher. Please see in attached file!</P>", obj_OrderInfo.CustomerName);
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