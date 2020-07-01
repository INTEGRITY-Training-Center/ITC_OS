using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using OnlineShopping.Controllers;
using OnlineShopping.Models;
using OnlineShopping.Properties;

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
                        ProductController productControl = new ProductController();
                        ProductDiscountController productDiscountController = new ProductDiscountController();

                        List<CartInfo> lstCartInfo = new List<CartInfo>();
                        List<CartInfo> lstCart = GettingJson(customerID);
                        decimal d_total = 0;
                        foreach (CartInfo obj in lstCart)
                        {
                            obj.ProductImage = productControl.GetProductImagebyID(obj.ProductID);
                            obj.DiscountAmount = productDiscountController.GetDiscountByProductID(obj.ProductID);
                            obj.TotalPrice = obj.Quantity * (obj.ProductPrice - obj.DiscountAmount);
                            d_total += obj.TotalPrice;

                            lstCartInfo.Add(obj);
                        }
                        DataBindToPage(lstCartInfo, d_total, customerID);
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

        public void DataBindToPage(List<CartInfo> lstCartInfo, decimal d_total, string customerID)
        {
            productList.DataSource = lstCartInfo;
            productList.DataBind();

            if (lstCartInfo.Count <= 0)
            {
                btnCreateOrder.Enabled = false;
            }
            else
            {
                btnCreateOrder.Enabled = true;
            }
            decimal taxPercentage = Settings.Default.CommercialTax;
            decimal commercialTaxAmt = decimal.Round(d_total * (taxPercentage / 100), 2);
            lblTax.Text = commercialTaxAmt.ToString();
            lblSubTotal.Text = decimal.Round(d_total, 2).ToString();// String.Format("{0:n}", d_total);  // Output: 1,234.00
            lblGrandTotal.Text = decimal.Round(d_total + commercialTaxAmt, 2).ToString();// String.Format("{0:n}", d_total);
            LabelTax.Text = "Tax (" + taxPercentage.ToString() + "%) : ";
            //Keep data
            txtCustomerID.Text = customerID;
            txtTax.Text = taxPercentage.ToString();
        }

        public static List<CartInfo> GettingJson(string CustomerID)
        {
            string cartFile = "~/CartJson/" + CustomerID + "_cart.json";
            string path = HttpContext.Current.Server.MapPath(cartFile);
            List<CartInfo> lstCart = new List<CartInfo>();
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                string jsonString = sr.ReadToEnd();
                sr.Close();
                JavaScriptSerializer ser = new JavaScriptSerializer();

                lstCart = ser.Deserialize<List<CartInfo>>(jsonString);
            }
            return lstCart;
        }

        public static void MakeJson(List<CartInfo> lstCart, string CustomerID)
        {
            string cartFile = "~/CartJson/" + CustomerID + "_cart.json";
            string path = HttpContext.Current.Server.MapPath(cartFile);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string res = JsonConvert.SerializeObject(lstCart);
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine(res.ToString());
                sw.Close();
            }
        }

        [WebMethod]
        public static string UpdateCart(string cart_id, int currentQty, string customer_id)
        {
            string result = "Success";
            List<CartInfo> lstCartInfo = new List<CartInfo>();
            List<CartInfo> lstCart = GettingJson(customer_id);
            foreach (CartInfo obj in lstCart)
            {
                if(obj.CartID.CompareTo(cart_id) == 0)
                {
                    obj.Quantity = currentQty;
                    obj.TotalPrice = currentQty * obj.ProductPrice;
                }
                lstCartInfo.Add(obj);
            }
            MakeJson(lstCartInfo, customer_id);
            return result;
        }

        protected void productList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string customerID = txtCustomerID.Text;
            var cartID = e.CommandArgument;

            if (e.CommandName == "Delete")
            {
                decimal d_total = 0; int delCartQty = 0;
                ProductController productControl = new ProductController();
                ProductDiscountController productDiscountController = new ProductDiscountController();

                List<CartInfo> lstCartInfo = new List<CartInfo>();
                List<CartInfo> lstCartInfoWithImage = new List<CartInfo>();
                List<CartInfo> lstCart = GettingJson(customerID);
                foreach (CartInfo obj in lstCart)
                {
                    if (obj.CartID != cartID.ToString())
                    {
                        //For Json file, can't add image byty[] property.
                        CartInfo obj_cart = new CartInfo();
                        obj_cart.CartID = obj.CartID;
                        obj_cart.ProductID = obj.ProductID;
                        obj_cart.ProductName = obj.ProductName;
                        obj_cart.Quantity = obj.Quantity;
                        obj_cart.ProductPrice = obj.ProductPrice;
                        obj_cart.TotalPrice = obj.TotalPrice;
                        obj_cart.CustomerID = obj.CustomerID;
                        lstCartInfo.Add(obj_cart);

                        obj.ProductImage = productControl.GetProductImagebyID(obj.ProductID);
                        obj.DiscountAmount = productDiscountController.GetDiscountByProductID(obj.ProductID);
                        obj.TotalPrice = obj.Quantity * (obj.ProductPrice - obj.DiscountAmount);
                        lstCartInfoWithImage.Add(obj);

                        d_total += obj.TotalPrice;
                    }
                    else
                    {
                        delCartQty = obj.Quantity;
                    }
                }

                Label lbl = (Label)(this.Master).FindControl("lblCartQty");
                int cartQty = Convert.ToInt32(String.IsNullOrEmpty(lbl.Text) ? "0" : lbl.Text);
                if((cartQty - delCartQty) > 0)
                {
                    lbl.Text = (cartQty - delCartQty).ToString();
                }
                else
                {
                    lbl.Text = "";
                }
               

                MakeJson(lstCartInfo, customerID);
                
                DataBindToPage(lstCartInfoWithImage, d_total, customerID);
            }
        }
        //Need to create because of "productList_ItemCommand" event
        protected void productList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            int orderQty = 0;decimal totalAmount = 0;
            string customerID = txtCustomerID.Text;
            decimal taxPercentage = Convert.ToDecimal(txtTax.Text);

            //============ Detail Data Preparation ============
            ProductDiscountController productDiscountController = new ProductDiscountController();
            List<OrderDetail> lst_OrderDetailInfo = new List<OrderDetail>();
            List<CartInfo> lstCart = GettingJson(customerID);
            foreach (CartInfo obj in lstCart)
            {
                orderQty += obj.Quantity;
                OrderDetail obj_OrderDetail = new OrderDetail();
                obj_OrderDetail.ProductID = obj.ProductID;
                obj_OrderDetail.Quantity = obj.Quantity;
                obj_OrderDetail.Price = obj.ProductPrice;
                obj_OrderDetail.DiscountAmount = productDiscountController.GetDiscountByProductID(obj.ProductID);
                lst_OrderDetailInfo.Add(obj_OrderDetail);

                totalAmount += obj.Quantity * (obj.ProductPrice - obj.DiscountAmount);
            }
            decimal commercialTax = totalAmount * (taxPercentage / 100);
            //============ Header Data Preparation ============
            OrderInfo obj_OrderInfo = new OrderInfo();
            obj_OrderInfo.OrderDate = DateTime.Now;
            obj_OrderInfo.OrderQuantity = orderQty;
            obj_OrderInfo.Tax = commercialTax;//When I get directly from lable, I can't get last changes amount.
            obj_OrderInfo.OrderAmount = totalAmount + commercialTax;// Convert.ToDecimal(lblGrandTotal.Text); When I get directly from lable, I can't get last changes amount.
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

                //============ Delete Json File ============
                File.Delete(Server.MapPath("~/CartJson/" + customerID + "_cart.json"));
                ExportOrderVoucher(return_OrderInfo.OrderID, return_OrderInfo.OrderNo);
            }

            Response.Redirect("OrderInfoList.aspx");
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