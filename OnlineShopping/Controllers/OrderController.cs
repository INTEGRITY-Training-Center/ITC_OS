using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopping.Models;
using OnlineShopping.Data;
using System.Data;

namespace OnlineShopping.Controllers
{
    public class OrderController
    {
        public OrderInfo InsertOrder(OrderInfo obj_order)
        {
            Guid id = Guid.NewGuid();
            obj_order.OrderID = id.ToString();
            obj_order.OrderNo = CreateOrderNo();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_OrderInfo tbl_order = new tbl_OrderInfo();
                tbl_order.OrderID = obj_order.OrderID;
                tbl_order.OrderNo =  obj_order.OrderNo;
                tbl_order.OrderDate = obj_order.OrderDate;
                tbl_order.OrderQuantity = obj_order.OrderQuantity;
                tbl_order.OrderAmount = obj_order.OrderAmount;
                tbl_order.DiscountAmount = obj_order.DiscountAmount;
                tbl_order.Tax = obj_order.Tax;
                tbl_order.OrderDescription = obj_order.OrderDescription;
                tbl_order.OrderStatus = obj_order.OrderStatus;
                tbl_order.CustomerID = obj_order.CustomerID;
                db.tbl_OrderInfos.InsertOnSubmit(tbl_order);
                db.SubmitChanges();
            }
            return obj_order;
        }

        public string CreateOrderNo()
        {
            int i_OrderCount = 0;
            string orderNo = "";

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                i_OrderCount = (from order in db.tbl_OrderInfos select order).Count();
            }
            i_OrderCount += 1;
            orderNo = i_OrderCount.ToString("D6");

            return orderNo;
        }

        public DataTable GetAllOrderInfo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(string));
            dt.Columns.Add("OrderNo", typeof(string));
            dt.Columns.Add("OrderDate", typeof(DateTime));
            dt.Columns.Add("CustomerID", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("OrderQuantity", typeof(int));
            dt.Columns.Add("OrderAmount", typeof(decimal));
            dt.Columns.Add("CustomerAddress", typeof(string));
            dt.Columns.Add("OrderDescription", typeof(string));
            dt.Columns.Add("OrderStatus", typeof(string));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllOrderInfo();
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["OrderID"] = obj.OrderID;
                    dr["OrderNo"] = obj.OrderNo;
                    dr["OrderDate"] = obj.OrderDate;
                    dr["CustomerID"] = obj.CustomerID;
                    dr["CustomerName"] = obj.CustomerName;
                    dr["OrderQuantity"] = obj.OrderQuantity;
                    dr["OrderAmount"] = obj.OrderAmount;
                    dr["CustomerAddress"] = obj.CustomerAddress;
                    dr["OrderDescription"] = obj.OrderDescription;
                    dr["OrderStatus"] = obj.OrderStatus;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public DataTable GetAllOrderByCustomerID(string customerID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(string));
            dt.Columns.Add("OrderNo", typeof(string));
            dt.Columns.Add("OrderDate", typeof(DateTime));
            dt.Columns.Add("CustomerID", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("OrderQuantity", typeof(int));
            dt.Columns.Add("OrderAmount", typeof(decimal));
            dt.Columns.Add("CustomerAddress", typeof(string));
            dt.Columns.Add("OrderDescription", typeof(string));
            dt.Columns.Add("OrderStatus", typeof(string));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetAllOrderByCustomerID(customerID);
                foreach (var obj in data)
                {
                    DataRow dr = dt.NewRow();
                    dr["OrderID"] = obj.OrderID;
                    dr["OrderNo"] = obj.OrderNo;
                    dr["OrderDate"] = obj.OrderDate;
                    dr["CustomerID"] = obj.CustomerID;
                    dr["CustomerName"] = obj.CustomerName;
                    dr["OrderQuantity"] = obj.OrderQuantity;
                    dr["OrderAmount"] = obj.OrderAmount;
                    dr["CustomerAddress"] = obj.CustomerAddress;
                    dr["OrderDescription"] = obj.OrderDescription;
                    dr["OrderStatus"] = obj.OrderStatus;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public OrderInfo GetOrderByID(string OrderID)
        {
            OrderInfo obj_Order = new OrderInfo();
            using(OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = db.sp_GetOrderByID(OrderID);
                foreach (var obj in data)
                {
                    obj_Order.OrderID  = obj.OrderID;
                    obj_Order.OrderNo  = obj.OrderNo;
                    obj_Order.OrderDate  = obj.OrderDate;
                    obj_Order.CustomerID  = obj.CustomerID;
                    obj_Order.CustomerName  = obj.CustomerName;
                    obj_Order.OrderQuantity  = obj.OrderQuantity;
                    obj_Order.OrderAmount  = obj.OrderAmount;
                    obj_Order.CustomerAddress  = obj.CustomerAddress;
                    obj_Order.CustomerAddress = obj.CustomerAddress;
                    obj_Order.OrderDescription  = obj.OrderDescription;
                    obj_Order.OrderStatus  = obj.OrderStatus;
                    obj_Order.CustomerEmail = obj.CustomerEmail;
                }
            }

            return obj_Order;
        }

        public void UpdateOrderStatus(OrderInfo obj_orderInfo)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_OrderInfo table_orderinfo = (from a in db.tbl_OrderInfos where a.OrderID == obj_orderInfo.OrderID select a).FirstOrDefault();
                if (table_orderinfo != null)
                {
                    table_orderinfo.OrderStatus = obj_orderInfo.OrderStatus;
                    db.SubmitChanges();
                }
            }
        }
    }
}