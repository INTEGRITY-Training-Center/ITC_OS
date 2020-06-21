using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class AddProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (fluPicture.PostedFile != null && fluPicture.PostedFile.ContentLength > 0)
                UpLoadAndDisplay();
        }

        private void UpLoadAndDisplay()
        {
            string imgPath = "~/Images/" + fluPicture.FileName;

            if (fluPicture.PostedFile != null && fluPicture.PostedFile.FileName != "")
            {
                lblImagePath.Text = Server.MapPath(imgPath);
                fluPicture.SaveAs(Server.MapPath(imgPath));
                imgPicture.ImageUrl = imgPath;
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            //can use this code when use file upload direcly without showing image.
            //HttpPostedFile p_file = fluPicture.PostedFile;//To create a PostedFile
            //Byte[] imgByte = new Byte[p_file.ContentLength];//Create byte Array with file len
            //p_file.InputStream.Read(imgByte, 0, p_file.ContentLength);//force the control to load data in array

            string imagePath = lblImagePath.Text;
            if (imagePath.Length > 0)
            {
                ProductController obj_product_control = new ProductController();
                Product obj_product = new Product();
                obj_product.ProductName = txtProductName.Text.Trim();
                obj_product.ProductCategory = ddlProductCategory.SelectedValue;
                obj_product.ProductPrice = Convert.ToDecimal(txtPrice.Text.Trim());
                obj_product.ProductDescription = txtDescription.Text.Trim();
                obj_product.ProductImage = ReadImage(imagePath);//imgByte;
                obj_product_control.InsertProduct(obj_product);

                if (File.Exists(imagePath))//IF old file exist, delete it.
                {
                    File.Delete(imagePath);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock
                    (this, this.GetType(), "alertMessage", "alert('You need to choose Image file')", true);
            }
        }

        private static byte[] ReadImage(string p_postedImageFileName)
        {
            FileStream fs = new FileStream(p_postedImageFileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] image = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return image;
        }
    }
}