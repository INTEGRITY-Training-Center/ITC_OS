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
                        DataBindToDDL(customerID);
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

        public void DataBindToDDL(string customerID)
        {
            decimal deliveryCharges = 0;

            //Get Login Customer's Township
            CustomerController customerController = new CustomerController();
            string CustomerTownship = customerController.GetTownshipByCustomerID(customerID);
            txtCustomerTownship.Text = CustomerTownship;

            DeliFeesController deliFeesController = new DeliFeesController();
            DataTable dt_township = deliFeesController.GetAllTownship();
            for (int i = 0; i < dt_township.Rows.Count; i++)
            {
                //Get Delivery Charges by Login Customer's Township
                if (CustomerTownship.CompareTo(dt_township.Rows[i]["Township"].ToString()) == 0)
                {
                    deliveryCharges = Convert.ToDecimal(dt_township.Rows[i]["DeliFees"].ToString());
                }

                ddlTownship.Items.Add((i).ToString());
                ddlTownship.Items[i].Text = dt_township.Rows[i]["Township"].ToString();
                //When value are same, we can get wrong selected text because of same value. So combine "Township" and "Fees".
                ddlTownship.Items[i].Value = dt_township.Rows[i]["Township"].ToString() + "-" + dt_township.Rows[i]["DeliFees"].ToString();
            }

            ddlTownship.Items.FindByText(CustomerTownship).Selected = true;
            lblDeliveryCharges.Text = decimal.Round(deliveryCharges, 2).ToString(); 
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
            decimal deliveryCharges = Convert.ToDecimal(lblDeliveryCharges.Text);
            lblTax.Text = commercialTaxAmt.ToString();
            lblSubTotal.Text = decimal.Round(d_total, 2).ToString();// String.Format("{0:n}", d_total);  // Output: 1,234.00
            lblGrandTotal.Text = decimal.Round(d_total + commercialTaxAmt + deliveryCharges, 2).ToString();// String.Format("{0:n}", d_total);
            LabelTax.Text = "Tax (" + taxPercentage.ToString() + "%)";

            //Keep data
            txtCustomerID.Text = customerID;
            txtTax.Text = taxPercentage.ToString();
        }

        protected void ddlTownship_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal d_SubTotal = Convert.ToDecimal(lblSubTotal.Text);
            decimal d_Tax = Convert.ToDecimal(lblTax.Text);
            string[] selectedValue = (ddlTownship.Items[ddlTownship.SelectedIndex].Value).Split('-');
            decimal deliveryCharges = Convert.ToDecimal(selectedValue[1]);

            lblDeliveryCharges.Text = decimal.Round(deliveryCharges, 2).ToString();
            lblGrandTotal.Text = decimal.Round(d_SubTotal + d_Tax + deliveryCharges, 2).ToString();

            if (txtCustomerTownship.Text.CompareTo(ddlTownship.Items[ddlTownship.SelectedIndex].Text) == 0)
            {
                txtOrderDescription.Text = "";
            }
            else
            {
                txtOrderDescription.Text = "Changes Delivery Address : " + ddlTownship.Items[ddlTownship.SelectedIndex].Text;
            }
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
            int orderQty = 0;decimal totalAmount = 0, discountAmount = 0;
            string customerID = txtCustomerID.Text;
            decimal taxPercentage = Convert.ToDecimal(txtTax.Text);

            //============ Detail Data Preparation ============
            ProductDiscountController productDiscountController = new ProductDiscountController();
            List<OrderDetail> lst_OrderDetailInfo = new List<OrderDetail>();
            List<CartInfo> lstCart = GettingJson(customerID);
            foreach (CartInfo obj in lstCart)
            {
                if(obj.Quantity > 0)
                {
                    orderQty += obj.Quantity;
                    discountAmount = productDiscountController.GetDiscountByProductID(obj.ProductID);

                    OrderDetail obj_OrderDetail = new OrderDetail();
                    obj_OrderDetail.ProductID = obj.ProductID;
                    obj_OrderDetail.Quantity = obj.Quantity;
                    obj_OrderDetail.Price = obj.ProductPrice;
                    obj_OrderDetail.DiscountAmount = discountAmount;
                    lst_OrderDetailInfo.Add(obj_OrderDetail);

                    totalAmount += obj.Quantity * (obj.ProductPrice - discountAmount);
                }                
            }
            decimal commercialTax = decimal.Round(totalAmount * (taxPercentage / 100),2);
            string[] selectedValue = (ddlTownship.Items[ddlTownship.SelectedIndex].Value).Split('-');
            decimal deliveryCharges = Convert.ToDecimal(selectedValue[1]);

            //============ Header Data Preparation ============
            OrderInfo obj_OrderInfo = new OrderInfo();
            obj_OrderInfo.OrderDate = DateTime.Now;
            obj_OrderInfo.OrderQuantity = orderQty;
            obj_OrderInfo.Tax = commercialTax;//When I get directly from lable, I can't get last changes amount.
            obj_OrderInfo.DiscountAmount = Convert.ToDecimal(0);//Will add control later.
            obj_OrderInfo.DeliveryCharges = deliveryCharges;
            obj_OrderInfo.OrderAmount = totalAmount + commercialTax + deliveryCharges;//When I get directly from lable, I can't get last changes amount.
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

    }
}