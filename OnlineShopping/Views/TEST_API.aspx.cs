using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineShopping.Views
{
    public partial class TEST_API : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        public class tbl_Expense
        {
            public string ExpenseID { get; set; }
            public string ItemName { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal? Amount { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public string PaymentType { get; set; }
            public string Currency { get; set; }
        }

        public async void BindData()
        {
            var httpClient = new HttpClient();
            dynamic resm = await httpClient.GetStringAsync("http://localhost:59877/api/tbl_Expense");
            gvExpenses.DataSource = JsonConvert.DeserializeObject<List<tbl_Expense>>(resm);
            gvExpenses.DataBind();
        }
    }
}