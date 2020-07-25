using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class DeliItemDetailInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["OrderID"].ToString() != null)
            {
                if (!IsPostBack)
                {
                    string OrderID = Request.QueryString["OrderID"].ToString();

                    DeliItemController deliItemController = new DeliItemController();
                    DeliItem obj_DeliItem = deliItemController.GetDeliItemByOrderID(OrderID);

                    order_id.InnerText = obj_DeliItem.OrderID;
                    txtOrderNo.Text = obj_DeliItem.OrderNo;
                    txtOrderQuantity.Text = obj_DeliItem.OrderQuantity.ToString();
                    txtOrderAmount.Text = obj_DeliItem.OrderAmount.ToString("###,###.00");
                    txtDeliveryCharges.Text = obj_DeliItem.DeliveryCharges.ToString("###,###.00");

                    txtCustomerName.Text = obj_DeliItem.CustomerName;
                    txtCustomerMobile.Text = obj_DeliItem.CustomerMobile;
                    txtCustomerAddress.Text = obj_DeliItem.CustomerAddress;
                    txtOrderDescription.Text = obj_DeliItem.OrderDescription;

                    string deliManName = obj_DeliItem.DeliMan_Name;
                    DataBindToDDL(deliManName);
                    txtDeliManMobile.Text = obj_DeliItem.DeliMan_Mobile;

                    if (obj_DeliItem.Status == true)
                    {
                        ddlDeliMan.Enabled = false;
                        btnAddDeliMan.Visible = false;
                    }
                }                    
            }
        }

        public void DataBindToDDL(string DeliManName)
        {
            DeliManController deliManController = new DeliManController();
            List<DeliMan> lst_DeliMan = deliManController.GetAllDeliMan();

            int i = 0;
            ddlDeliMan.Items.Add("-- Select Delivery Man --");
            foreach (DeliMan obj in lst_DeliMan)
            {    
                ddlDeliMan.Items.Add((i + 1).ToString());
                ddlDeliMan.Items[i + 1].Text = obj.DeliMan_Name;
                ddlDeliMan.Items[i + 1].Value = obj.DeliMan_Mobile;
                i++;
            }

            if(String.IsNullOrEmpty(DeliManName))
            {
                ddlDeliMan.Items.FindByText("-- Select Delivery Man --").Selected = true;
            }
            else
            {
                ddlDeliMan.Items.FindByText(DeliManName).Selected = true;
                txtDeliManMobile.Text = ddlDeliMan.Items[ddlDeliMan.SelectedIndex].Value;
            }           
        }

        protected void ddlDeliMan_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDeliManMobile.Text = ddlDeliMan.Items[ddlDeliMan.SelectedIndex].Value;
        }

        protected void btnAddDeliMan_Click(object sender, EventArgs e)
        {
            DeliItemController deliItemController = new DeliItemController();
            DeliManController deliManController = new DeliManController();
            DeliMan obj_DeliMan = deliManController.GetDeliManBy_Name_Mobile(ddlDeliMan.Items[ddlDeliMan.SelectedIndex].Text, ddlDeliMan.Items[ddlDeliMan.SelectedIndex].Value);

            DeliItem obj_DeliItem = new DeliItem();
            obj_DeliItem.OrderID = order_id.InnerText;
            obj_DeliItem.DeliManID = obj_DeliMan.DeliManID;

            // Update Deli Item for Deli Man
            deliItemController.UpdateDeliMan(obj_DeliItem);

            Response.Redirect("DeliItemList.aspx");
        }

    }
}