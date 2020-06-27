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
using System.Data;

namespace OnlineShopping.Views
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Label lbl = (Label)(this.Master).FindControl("lblCartQty");
            //lbl.Text = "10";

            ProductController productControl = new ProductController();
            productList.DataSource = productControl.GetAllProducts();
            productList.DataBind();
        }

        public  void CreateJson(CartInfo objCartInfo)
        {
            string cartFile = "~/CartJson/" + objCartInfo.CustomerID + "_cart.json";
            string path = Server.MapPath(cartFile);
            List<CartInfo> lstCart = new List<CartInfo>();
            if (File.Exists(path))
            {                
                StreamReader sr = new StreamReader(path);
                string jsonString = sr.ReadToEnd();
                sr.Close();
                JavaScriptSerializer ser = new JavaScriptSerializer();
                
                lstCart = ser.Deserialize<List<CartInfo>>(jsonString);

                lstCart.Add(objCartInfo);

                File.Delete(path);
                MakeJson(lstCart, path);
            }
            else if (!File.Exists(path))
            {
                lstCart.Add(objCartInfo);
                MakeJson(lstCart, path);
            }
        }

        public void MakeJson(List<CartInfo> lstCart, String cartFilePath)
        {
           string res = JsonConvert.SerializeObject(lstCart);
            using (var sw = new StreamWriter(cartFilePath, true))
            {
                sw.WriteLine(res.ToString());
                sw.Close();
            }
        }

        [WebMethod]
        public static string InsertCart(string product_id, string product_name, string product_price)
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
                    CartInfo obj_cart = new CartInfo();
                    Guid id = Guid.NewGuid();
                    obj_cart.CartID = id.ToString();
                    obj_cart.ProductID = product_id;
                    obj_cart.ProductName = product_name;
                    obj_cart.Quantity = Convert.ToInt32("1");
                    obj_cart.ProductPrice = Convert.ToDecimal(product_price);
                    obj_cart.TotalPrice = Convert.ToDecimal(product_price);
                    obj_cart.CustomerID = customerID;
                                       
                    Index ind = new Index();
                    ind.CreateJson(obj_cart);
                    result = "Success";                   
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

        protected void txtSearching_TextChanged(object sender, EventArgs e)
        {
            List<Product> lst_product = (List<Product>)productList.DataSource;

            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("ProductCategory", typeof(string));
            dt.Columns.Add("ProductPrice", typeof(string));
            dt.Columns.Add("ProductDescription", typeof(string));
            dt.Columns.Add("ProductImage", typeof(byte[]));

            foreach(Product obj in lst_product)
            {
                DataRow dr = dt.NewRow();
                dr["ProductID"] = obj.ProductID;
                dr["ProductName"] = obj.ProductName;
                dr["ProductCategory"] = obj.ProductCategory;
                dr["ProductPrice"] = obj.ProductPrice;
                dr["ProductDescription"] = obj.ProductDescription;
                dr["ProductImage"] = obj.ProductImage.ToArray();
                dt.Rows.Add(dr);
            }

            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("ProductName Like '%{0}%'", txtSearching.Text);
            
            productList.DataSource = dv;
            productList.DataBind();
        }

        [WebMethod]
        public static List<Product> GetAllProductsByName(string ProductName)
        {
            ProductController productControl = new ProductController();
            List<Product> lstProducts = new List<Product>();
            lstProducts = productControl.GetAllProductsByName(ProductName);
            return lstProducts;
        }
    }
}