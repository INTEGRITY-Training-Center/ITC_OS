using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class OrderDetailController
    {
        public void InsertOrderDetail(string OrderID, List<OrderDetail> lst_OrderDetail)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                foreach (OrderDetail obj_OrderDetail in lst_OrderDetail)
                {
                    tbl_OrderDetail tbl_detail = (from a in db.tbl_OrderDetails
                                                  where a.ProductID == obj_OrderDetail.ProductID && a.OrderID == OrderID
                                                  select a).FirstOrDefault();
                    if(tbl_detail != null)
                    {
                        tbl_detail.Quantity = Convert.ToInt32(tbl_detail.Quantity) + obj_OrderDetail.Quantity;
                        db.SubmitChanges();
                    }
                    else
                    {
                        tbl_OrderDetail tbl_orderdetail = new tbl_OrderDetail();
                        tbl_orderdetail.OrderID = OrderID.ToString();
                        tbl_orderdetail.ProductID = obj_OrderDetail.ProductID;
                        tbl_orderdetail.Quantity = obj_OrderDetail.Quantity;
                        tbl_orderdetail.Price = obj_OrderDetail.Price;
                        tbl_orderdetail.DiscountAmount = obj_OrderDetail.DiscountAmount;

                        db.tbl_OrderDetails.InsertOnSubmit(tbl_orderdetail);
                        db.SubmitChanges();
                    }                    
                }
            }
        }

        public DataTable GetAllOrderDetailByOrderID(string OrderID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(string));
            dt.Columns.Add("ProductID", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("ProductImage", typeof(byte[]));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("ProductPrice", typeof(string));
            dt.Columns.Add("TotalPrice", typeof(string));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllOrderDetailByOrderID(OrderID);
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["OrderID"] = obj.OrderID;
                    dr["ProductID"] = obj.ProductID;
                    dr["ProductName"] = obj.ProductName;
                    dr["ProductImage"] = obj.ProductImage.ToArray();
                    dr["Quantity"] = obj.Quantity;
                    dr["ProductPrice"] = obj.ProductPrice;
                    dr["TotalPrice"] = obj.TotalPrice;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

    }
}