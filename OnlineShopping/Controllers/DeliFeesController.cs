using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class DeliFeesController
    {
        public DataTable GetAllTownship()
        {
            DataTable dt_township = new DataTable();
            dt_township.Columns.Add("Township", typeof(string));
            dt_township.Columns.Add("DeliFees", typeof(decimal));

            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = (from a in db.tbl_Deli_Fees orderby a.Township select a).ToList();
                foreach (var obj in data)
                {
                    DataRow dr = dt_township.NewRow();
                    dr["Township"] = obj.Township;
                    dr["DeliFees"] = obj.Deli_Fees;
                    dt_township.Rows.Add(dr);
                }
            }

            return dt_township;
        }
    }
}