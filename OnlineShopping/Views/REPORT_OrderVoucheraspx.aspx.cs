using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using OnlineShopping.Controllers;
using OnlineShopping.Models;

namespace OnlineShopping.Views
{
    public partial class REPORT_OrderVoucheraspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["OrderID"].ToString() != null)
                {
                    string OrderID = Request.QueryString["OrderID"].ToString(); ;
                    OrderController order_controller = new OrderController();
                    OrderInfo obj_OrderInfo = order_controller.GetOrderByID(OrderID);
                    //Header Parameters
                    ReportParameter rp_orderno = new ReportParameter("p_orderno", obj_OrderInfo.OrderNo);
                    ReportParameter rp_order_date = new ReportParameter("p_order_date", String.Format("{0:dd-MMM-yyyy}", obj_OrderInfo.OrderDate));
                    ReportParameter rp_delivery_date = new ReportParameter("p_delivery_date", obj_OrderInfo.EstDeliveryDate.ToString());
                    ReportParameter rp_order_quantity = new ReportParameter("p_order_quantity", obj_OrderInfo.OrderQuantity.ToString());
                    ReportParameter rp_customer_name = new ReportParameter("p_customer_name", obj_OrderInfo.CustomerName);
                    ReportParameter rp_customer_address = new ReportParameter("p_customer_address", obj_OrderInfo.CustomerAddress);
                    ReportParameter rp_customer_mobile = new ReportParameter("p_customer_mobile", obj_OrderInfo.CustomerMobile);
                    ReportParameter rp_additional_request = new ReportParameter("p_additional_request", obj_OrderInfo.OrderDescription);
                    //Total Parameters
                    //
                    decimal subTotal = Convert.ToDecimal(obj_OrderInfo.OrderAmount - obj_OrderInfo.Tax - obj_OrderInfo.DeliveryCharges);

                    ReportParameter rp_sub_total = new ReportParameter("p_sub_total", String.Format("{0:##,###.00}", subTotal.ToString()));
                    ReportParameter rp_tax = new ReportParameter("p_tax", obj_OrderInfo.Tax.ToString());
                    ReportParameter rp_delivery_charges = new ReportParameter("p_delivery_charges", obj_OrderInfo.DeliveryCharges.ToString());
                    ReportParameter rp_grand_total = new ReportParameter("p_grand_total", obj_OrderInfo.OrderAmount.ToString());
                    //Report DataSet
                    OrderDetailController order_Detail_controller = new OrderDetailController();
                    ReportDataSource rds = new ReportDataSource("DataSet1", order_Detail_controller.GetAllOrderDetailByOrderID(OrderID));

                    rvOrderVoucher.Reset();
                    rvOrderVoucher.LocalReport.DataSources.Clear();
                    rvOrderVoucher.LocalReport.ReportPath = Server.MapPath("RDLC_OrderVoucher.rdlc");
                    rvOrderVoucher.LocalReport.SetParameters(new ReportParameter[] { rp_orderno, rp_order_date, rp_delivery_date, rp_order_quantity, rp_customer_name, rp_customer_address, rp_customer_mobile, rp_additional_request, rp_sub_total, rp_delivery_charges, rp_grand_total, rp_tax });
                    rvOrderVoucher.LocalReport.DataSources.Add(rds);
                    rvOrderVoucher.DataBind();
                    rvOrderVoucher.LocalReport.Refresh();

                }
            }
        }
    }
}