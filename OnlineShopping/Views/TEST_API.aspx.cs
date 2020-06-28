using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            var httpClient = new HttpClient();//using System.Net.Http; for HttpClient
            dynamic resm = await httpClient.GetStringAsync("http://localhost:3333/itc/getExpense");
            gvExpenses.DataSource = JsonConvert.DeserializeObject<List<tbl_Expense>>(resm);
            gvExpenses.DataBind();
        }

        protected async void btnAddExpense_Click(object sender, EventArgs e)
        {
            tbl_Expense objExpense = new tbl_Expense();
            Guid id = Guid.NewGuid();
            objExpense.ExpenseID = id.ToString();
            objExpense.ItemName = txtItemName.Text.Trim();
            objExpense.Category = ddlCategory.SelectedValue;
            objExpense.Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
            objExpense.Price = Convert.ToDecimal(txtPrice.Text.Trim());
            objExpense.Amount = Convert.ToInt32(txtQuantity.Text.Trim()) * Convert.ToDecimal(txtPrice.Text.Trim());
            objExpense.PaymentType = ddlPaymentType.SelectedValue;
            objExpense.Currency = ddlCurrency.SelectedValue;
            objExpense.Date = Convert.ToDateTime(expenseDate.Value);
            objExpense.Description = txtDescription.Text.Trim();

            try
            {
                var json = JsonConvert.SerializeObject(objExpense);
                var content = new StringContent(json, Encoding.UTF8, "application/json");//using System.Text; for Encoding

                var httpClient = new HttpClient();
                var request = await httpClient.PostAsync("http://localhost:3333/itc/addExpense", content);
                request.EnsureSuccessStatusCode();
                //ClientScript.RegisterStartupScript //find in online
            }
            catch(Exception ex)
            {
                lblResult.Text = "Fail : " + ex;
            }
            finally
            {
                Response.Redirect(Request.RawUrl);
            }
        }

        protected async void btnDeleteExpense_Click(object sender, EventArgs e)
        {
            try
            {
                var httpClient = new HttpClient();
                var request = await httpClient.DeleteAsync("http://localhost:3333/itc/deleteExpense?id=" + txtExpenseID.Text.Trim());
            }
            catch (Exception ex)
            {
                lblResult.Text = "Fail : " + ex;
            }
            finally
            {
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}