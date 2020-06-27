using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class OnlieShopping : System.Web.UI.MasterPage
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

            if (!String.IsNullOrEmpty(CustomerID))
            {
                la.InnerHtml = "Logout";
                int cartQty = 0;
                List<CartInfo> lstCart = GettingJson(CustomerID);
                if (lstCart.Count > 0)
                {
                    foreach(CartInfo obj in lstCart)
                    {
                        cartQty += obj.Quantity;
                    }
                    lblCartQty.Text = cartQty.ToString();
                }
            }
            else
            {
                la.InnerHtml = "Login";
            }

            //already login and login user is admin
            if (!String.IsNullOrEmpty(CustomerID) && (!String.IsNullOrEmpty(IsAdmin) && Convert.ToBoolean(IsAdmin) == true))
            {
                productLink.Visible = true;
                orderLink.Visible = true;
                customerLink.Visible = true;
                wishlistLink.Visible = false;
                myOrderLink.Visible = false;
            }
            else
            {
                productLink.Visible = false;
                orderLink.Visible = false;
                customerLink.Visible = false;
                wishlistLink.Visible = true;
                myOrderLink.Visible = true;
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
    }
}