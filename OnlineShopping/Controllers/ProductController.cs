using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class ProductController
    {
        public void InsertProduct(Product obj_product)
        {
            using(OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Product tbl_product = new tbl_Product();
                Guid id = Guid.NewGuid();
                tbl_product.ProductID = id.ToString();
                tbl_product.ProductName = obj_product.ProductName;
                tbl_product.ProductCategory = obj_product.ProductCategory;
                tbl_product.ProductPrice = obj_product.ProductPrice;
                tbl_product.ProductImage = obj_product.ProductImage;
                tbl_product.ProductDescription = obj_product.ProductDescription;

                db.tbl_Products.InsertOnSubmit(tbl_product);
                db.SubmitChanges();
            }
        }

        public void UpdateProduct(Product obj_product)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Product tbl_product = (from a in db.tbl_Products 
                                  where a.ProductID == obj_product.ProductID //&& a.ProductName == obj_product.ProductName 
                                  select a).FirstOrDefault();

                tbl_product.ProductDescription = obj_product.ProductDescription;
                db.SubmitChanges();
            }
        }

        public List<Product> GetAllProducts()
        {
            List<Product> lst_products = new List<Product>();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = (from a in db.tbl_Products orderby a.ProductName select a).ToList();
                foreach (var obj in data)
                {
                    Product obj_product = new Product();
                    obj_product.ProductID = obj.ProductID;
                    obj_product.ProductName = obj.ProductName;
                    obj_product.ProductCategory = obj.ProductCategory;
                    obj_product.ProductPrice = obj.ProductPrice;
                    obj_product.ProductDescription = obj.ProductDescription;
                    obj_product.ProductImage = obj.ProductImage.ToArray();

                    lst_products.Add(obj_product);
                }
            }

            return lst_products;
        }
    }
}