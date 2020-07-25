using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class DeliItemController
    {
        public void InsertDeliItem(DeliItem obj_DeliITem)
        {
            Guid id = Guid.NewGuid();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_DeliItemID tbl_deliItem = new tbl_DeliItemID();
                tbl_deliItem.DeliItemID = id.ToString();
                tbl_deliItem.OrderID = obj_DeliITem.OrderID;
                tbl_deliItem.Status = obj_DeliITem.Status;
                tbl_deliItem.EstDeliveryDate = obj_DeliITem.EstDeliveryDate;
                tbl_deliItem.CreatedDate = obj_DeliITem.CreatedDate;
                tbl_deliItem.UpdatedDate = obj_DeliITem.UpdatedDate;
                tbl_deliItem.OrderNo = obj_DeliITem.OrderNo;
                tbl_deliItem.OrderAmount = obj_DeliITem.OrderAmount;
                tbl_deliItem.Tax = obj_DeliITem.Tax;
                tbl_deliItem.DiscountAmount = obj_DeliITem.DiscountAmount;
                tbl_deliItem.DeliveryCharges = obj_DeliITem.DeliveryCharges;
                tbl_deliItem.OrderQuantity = obj_DeliITem.OrderQuantity;
                tbl_deliItem.CustomerName = obj_DeliITem.CustomerName;
                tbl_deliItem.CustomerMobile = obj_DeliITem.CustomerMobile;
                tbl_deliItem.CustomerAddress = obj_DeliITem.CustomerAddress;
                tbl_deliItem.OrderDescription = obj_DeliITem.OrderDescription;
                db.tbl_DeliItemIDs.InsertOnSubmit(tbl_deliItem);
                db.SubmitChanges();
            }
        }

        public DataTable GetAllDeliItem()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DeliItemID", typeof(string));
            dt.Columns.Add("OrderID", typeof(string));            
            dt.Columns.Add("OrderNo", typeof(string));
            dt.Columns.Add("OrderQuantity", typeof(int));
            dt.Columns.Add("OrderAmount", typeof(string));
            dt.Columns.Add("DiscountAmount", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("DeliveryCharges", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("CustomerMobile", typeof(string));
            dt.Columns.Add("CustomerAddress", typeof(string));
            dt.Columns.Add("OrderDescription", typeof(string));
            dt.Columns.Add("EstDeliveryDate", typeof(DateTime));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("UpdatedDate", typeof(string));
            dt.Columns.Add("DeliManID", typeof(string));
            dt.Columns.Add("DeliMan_Name", typeof(string));
            dt.Columns.Add("DeliMan_Mobile", typeof(string));
            dt.Columns.Add("Status", typeof(bool));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllDeliItem();
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["DeliItemID"] = obj.DeliItemID;
                    dr["OrderID"] = obj.OrderID;
                    dr["OrderNo"] = obj.OrderNo;
                    dr["OrderQuantity"] = obj.OrderQuantity;
                    dr["OrderAmount"] = Convert.ToDecimal(obj.OrderAmount).ToString("###,###.00");
                    dr["DiscountAmount"] = obj.DiscountAmount;
                    dr["Tax"] = obj.Tax;
                    dr["DeliveryCharges"] = Convert.ToDecimal(obj.DeliveryCharges).ToString("###,###.00");
                    dr["CustomerName"] = obj.CustomerName;
                    dr["CustomerMobile"] = obj.CustomerMobile;
                    dr["CustomerAddress"] = obj.CustomerAddress;
                    dr["OrderDescription"] = obj.OrderDescription;
                    dr["EstDeliveryDate"] = obj.EstDeliveryDate;
                    dr["CreatedDate"] = obj.CreatedDate; 
                    if (obj.Status == false)
                    {
                        dr["UpdatedDate"] = "";
                    }
                    else
                    {
                        dr["UpdatedDate"] = Convert.ToDateTime(obj.UpdatedDate).ToString("dd-MMM-yyyy");
                    }
                    dr["DeliManID"] = obj.DeliManID;
                    dr["DeliMan_Name"] = obj.DeliMan_Name;
                    dr["DeliMan_Mobile"] = obj.DeliMan_Mobile;
                    dr["Status"] = obj.Status;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public DeliItem GetDeliItemByOrderID(string OrderID)
        {
            DeliItem obj_DeliItem = new DeliItem();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetDeliItemByOrderID(OrderID);
                foreach (var obj in data)
                {
                    obj_DeliItem.DeliItemID  = obj.DeliItemID;
                    obj_DeliItem.OrderID  = obj.OrderID;
                    obj_DeliItem.OrderNo  = obj.OrderNo;
                    obj_DeliItem.OrderQuantity  = obj.OrderQuantity;
                    obj_DeliItem.OrderAmount  = obj.OrderAmount;
                    obj_DeliItem.DiscountAmount  = obj.DiscountAmount;
                    obj_DeliItem.Tax  = obj.Tax;
                    obj_DeliItem.DeliveryCharges  = obj.DeliveryCharges;
                    obj_DeliItem.CustomerName  = obj.CustomerName;
                    obj_DeliItem.CustomerMobile  = obj.CustomerMobile;
                    obj_DeliItem.CustomerAddress  = obj.CustomerAddress;
                    obj_DeliItem.OrderDescription  = obj.OrderDescription;
                    obj_DeliItem.EstDeliveryDate  = obj.EstDeliveryDate;
                    obj_DeliItem.CreatedDate  = obj.CreatedDate;
                    obj_DeliItem.UpdatedDate = obj.UpdatedDate;
                    obj_DeliItem.DeliManID = obj.DeliManID;
                    obj_DeliItem.DeliMan_Name  = obj.DeliMan_Name;
                    obj_DeliItem.DeliMan_Mobile  = obj.DeliMan_Mobile;
                    obj_DeliItem.Status  = obj.Status;
                }
            }

            return obj_DeliItem;
        }

        public void UpdateDeliMan(DeliItem obj_DeliItem)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_DeliItemID table_DeliItem = (from a in db.tbl_DeliItemIDs where a.OrderID == obj_DeliItem.OrderID select a).FirstOrDefault();
                if (table_DeliItem != null)
                {
                    table_DeliItem.DeliManID = obj_DeliItem.DeliManID;
                    db.SubmitChanges();
                }
            }
        }
    }
}