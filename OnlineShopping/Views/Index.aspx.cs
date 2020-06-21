using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace OnlineShopping.Views
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string hostName = System.Net.Dns.GetHostName();
            ProductController productControl = new ProductController();
            productList.DataSource = productControl.GetAllProducts();
            productList.DataBind();
        }

        public  void CreateJson(CartInfo caInfo)
        {
            string path = Server.MapPath(caInfo.CustomerID + "_cart.json");
            List<CartInfo> dp;
            string res;
            if (File.Exists(path))
            {
                
                string jsonResult = JsonConvert.SerializeObject(caInfo);
                StreamReader sr = new StreamReader(Server.MapPath(caInfo.CustomerID + "_cart.json"));
                string jsonString = sr.ReadToEnd();
                sr.Close();
                JavaScriptSerializer ser = new JavaScriptSerializer();
                
                dp = ser.Deserialize<List<CartInfo>>(jsonString);

                dp.Add(caInfo);

                File.Delete(path);
                makeJSon(dp,caInfo);

            }



            else if (!File.Exists(path))
            {
                List<CartInfo> lstCart = new List<CartInfo>();
                lstCart.Add(caInfo);
                string jsonResult = JsonConvert.SerializeObject(lstCart);
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(jsonResult.ToString());
                    sw.Close();
                }
            }
        }

        public void makeJSon(List<CartInfo> rs, CartInfo ca)
        {
           string res = JsonConvert.SerializeObject(rs);
            using (var sw = new StreamWriter(Server.MapPath(ca.CustomerID + "_cart.json"), true))
            {
                sw.WriteLine(res.ToString());
                sw.Close();
            }
        }

        [WebMethod]
        public static string InsertCart(string product_id)
        {
            string result = "";
            string customerID = "";
            if (HttpContext.Current.Request.Cookies["CustomerID"] != null)
            {
                customerID = HttpContext.Current.Request.Cookies["CustomerID"].Value;
                if (customerID.Length <= 0)
                {
                    result = "Fail";
                }                    
                else
                {
                    CartController obj_cart_control = new CartController();
                    CartInfo obj_cart = new CartInfo();
                    obj_cart.ProductID = product_id;
                    obj_cart.CustomerID = customerID;
                    obj_cart.Quantity = Convert.ToInt32("1");
                    obj_cart_control.InsertCart(obj_cart);
                    result = "Success";
                    Index ind = new Index();
                    ind.CreateJson(obj_cart);
                }
            }
            else
            {
                result = "Fail";
            }
            return result;
        }

        [WebMethod]
        public static string InsertWishlist(string product_id)
        {
            string result = "";
            string customerID = "";
            if (HttpContext.Current.Request.Cookies["CustomerID"] != null)
            {
                customerID = HttpContext.Current.Request.Cookies["CustomerID"].Value;
                if (customerID.Length <= 0)
                {
                    result = "Fail";
                }
                else
                {
                    WishlistController obj_wishlist_control = new WishlistController();
                    Wishlist obj_wishlist = new Wishlist();
                    obj_wishlist.ProductID = product_id;
                    obj_wishlist.CustomerID = customerID;
                    obj_wishlist.AddedDate = DateTime.Now;
                    result = obj_wishlist_control.InsertWishlist(obj_wishlist);
                }
            }
            else
            {
                result = "Fail";
            }

            return result;
        }
    }
}