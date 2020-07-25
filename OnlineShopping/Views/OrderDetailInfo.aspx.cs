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
using OnlineShopping.Properties;

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
                
                string OrderID = Request.QueryString["OrderID"].ToString(); 
                OrderController orderControl = new OrderController();
                OrderInfo obj_OrderInfo = orderControl.GetOrderByID(OrderID);
                //Bind to Header
                order_id.InnerText = OrderID;
                lblOrderNo.Text = obj_OrderInfo.OrderNo;
                lblOrderDate.Text = String.Format("{0:dd-MMM-yyyy}", obj_OrderInfo.OrderDate);
                lblCustomerName.Text = obj_OrderInfo.CustomerName;
                lblCustomerMobile.Text = obj_OrderInfo.CustomerMobile;
                lblOrderQuantity.Text = obj_OrderInfo.OrderQuantity.ToString();
                lblCustomerAddress.Text = obj_OrderInfo.CustomerAddress;
                lblAdditionalRequest.Text = obj_OrderInfo.OrderDescription;
                if(obj_OrderInfo.EstDeliveryDate != null)
                {
                    lblDeliveryDate.Text = Convert.ToDateTime(obj_OrderInfo.EstDeliveryDate).ToString("dd-MMM-yyy");
                }
               
                //Bind to Total
                lblSubTotal.Text = (obj_OrderInfo.OrderAmount - obj_OrderInfo.Tax - obj_OrderInfo.DeliveryCharges).ToString();
                decimal taxPercentage = Settings.Default.CommercialTax;
                LabelTax.Text = "Tax (" + taxPercentage.ToString() + "%)";
                lblTax.Text = obj_OrderInfo.Tax.ToString();
                lblDeliveryCharges.Text = obj_OrderInfo.DeliveryCharges.ToString();
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
                        deliverydiv.Visible = true;
                        lbldeliverydiv.Visible = false;

                        btnCheckOrder.Enabled = true;
                        btnCheckOrder.Text = "Order Confirm";
                    }
                    else if (obj_OrderInfo.OrderStatus == "Confirmed")
                    {
                        deliverydiv.Visible = false;
                        lbldeliverydiv.Visible = true;

                        btnCheckOrder.Enabled = false;
                        btnCheckOrder.Text = "Order Confirmed";
                    }
                    else if (obj_OrderInfo.OrderStatus == "Delivered")
                    {
                        deliverydiv.Visible = false;
                        lbldeliverydiv.Visible = true;

                        btnCheckOrder.Enabled = false;
                        btnCheckOrder.Text = "Done";
                    }
                }
                else
                {
                    btnCheckOrder.Visible = false;
                    if (obj_OrderInfo.OrderStatus == "Created")
                    {
                        deliverydiv.Visible = false;
                        lbldeliverydiv.Visible = false;
                    }
                    else
                    {
                        deliverydiv.Visible = false;
                        lbldeliverydiv.Visible = true;
                    }
                }
            }
        }

        protected void btnCheckOrder_Click(object sender, EventArgs e)
        {
            OrderController orderControl = new OrderController();
            DeliItemController deliItemControl = new DeliItemController();

            //Prepare Order Info
            OrderInfo obj_orderInfo = new OrderInfo();
            obj_orderInfo.OrderID = order_id.InnerText;
            obj_orderInfo.OrderStatus = "Confirmed";
            obj_orderInfo.EstDeliveryDate = Convert.ToDateTime(deliveryDate.Value);

            //Prepare Deli Item Info
            DeliItem obj_deliItem = new DeliItem();
            obj_deliItem.OrderID = order_id.InnerText;
            obj_deliItem.Status = false;
            obj_deliItem.EstDeliveryDate = Convert.ToDateTime(deliveryDate.Value);
            obj_deliItem.CreatedDate = DateTime.Today;
            obj_deliItem.UpdatedDate = DateTime.Today;
            obj_deliItem.OrderNo = lblOrderNo.Text;
            obj_deliItem.OrderAmount = Convert.ToDecimal(lblGrandTotal.Text);
            obj_deliItem.Tax = Convert.ToDecimal(lblTax.Text);
            obj_deliItem.DiscountAmount = Convert.ToDecimal(0);//Will add control later.
            obj_deliItem.DeliveryCharges = Convert.ToDecimal(lblDeliveryCharges.Text);
            obj_deliItem.OrderQuantity = Convert.ToInt32(lblOrderQuantity.Text);
            obj_deliItem.CustomerName = lblCustomerName.Text;
            obj_deliItem.CustomerMobile = lblCustomerMobile.Text;
            obj_deliItem.CustomerAddress = lblCustomerAddress.Text;
            obj_deliItem.OrderDescription = lblAdditionalRequest.Text;

            // Update Order Info and Insert Deli Item
            orderControl.UpdateOrderStatus(obj_orderInfo);
            deliItemControl.InsertDeliItem(obj_deliItem);

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
            ReportParameter rp_delivery_date = new ReportParameter("p_delivery_date", obj_OrderInfo.EstDeliveryDate.ToString());
            ReportParameter rp_order_quantity = new ReportParameter("p_order_quantity", obj_OrderInfo.OrderQuantity.ToString());
            ReportParameter rp_customer_name = new ReportParameter("p_customer_name", obj_OrderInfo.CustomerName);
            ReportParameter rp_customer_address = new ReportParameter("p_customer_address", obj_OrderInfo.CustomerAddress);
            ReportParameter rp_customer_mobile = new ReportParameter("p_customer_mobile", obj_OrderInfo.CustomerMobile);
            ReportParameter rp_additional_request = new ReportParameter("p_additional_request", obj_OrderInfo.OrderDescription);
            //Total Parameters
            decimal subTotal = Convert.ToDecimal(obj_OrderInfo.OrderAmount - obj_OrderInfo.Tax - obj_OrderInfo.DeliveryCharges);

            ReportParameter rp_sub_total = new ReportParameter("p_sub_total", String.Format("{0:##,###.00}", subTotal.ToString()));
            ReportParameter rp_tax = new ReportParameter("p_tax", obj_OrderInfo.Tax.ToString());
            ReportParameter rp_delivery_charges = new ReportParameter("p_delivery_charges", obj_OrderInfo.DeliveryCharges.ToString());
            ReportParameter rp_grand_total = new ReportParameter("p_grand_total", obj_OrderInfo.OrderAmount.ToString());
            //Report DataSet
            OrderDetailController order_Detail_controller = new OrderDetailController();
            ReportDataSource rds = new ReportDataSource("DataSet1", order_Detail_controller.GetAllOrderDetailByOrderID(OrderID));

            ReportViewer rvOrderVoucher = new ReportViewer();
            rvOrderVoucher.LocalReport.Refresh();
            rvOrderVoucher.LocalReport.DataSources.Clear();
            rvOrderVoucher.LocalReport.ReportPath = Server.MapPath("RDLC_OrderVoucher.rdlc");
            rvOrderVoucher.LocalReport.SetParameters(new ReportParameter[] { rp_orderno, rp_order_date, rp_delivery_date, rp_order_quantity, rp_customer_name, rp_customer_address, rp_customer_mobile, rp_additional_request, rp_sub_total, rp_delivery_charges, rp_grand_total, rp_tax });
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