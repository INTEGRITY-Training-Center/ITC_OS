using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class CustomerController
    {
        public void InsertCustomer(Customer obj_customer)
        {
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Customer tbl_customer = new tbl_Customer();
                Guid id = Guid.NewGuid();
                tbl_customer.CustomerID = id.ToString();
                tbl_customer.CustomerName = obj_customer.CustomerName;
                tbl_customer.CustomerEmail = obj_customer.CustomerEmail;
                tbl_customer.CustomerMobile = obj_customer.CustomerMobile;
                tbl_customer.CustomerAddress = obj_customer.CustomerAddress;
                tbl_customer.CustomerTownship = obj_customer.CustomerTownship;
                tbl_customer.CustomerPassword = obj_customer.CustomerPassword;
                tbl_customer.IsAdmin = Convert.ToBoolean(obj_customer.IsAdmin);

                db.tbl_Customers.InsertOnSubmit(tbl_customer);
                db.SubmitChanges();
            }
        }
        public Customer CheckCustomerLogin(string name, string password)
        {
            Customer obj_customer = new Customer();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_Customer cus = (from a in db.tbl_Customers where a.CustomerName == name && a.CustomerPassword == password select a).FirstOrDefault();

                if (cus != null)
                {
                    obj_customer.CustomerID = cus.CustomerID;
                    obj_customer.CustomerName = cus.CustomerName;
                    obj_customer.CustomerEmail = cus.CustomerEmail;
                    obj_customer.CustomerMobile = cus.CustomerMobile;
                    obj_customer.CustomerAddress = cus.CustomerAddress;
                    obj_customer.CustomerPassword = cus.CustomerPassword;
                    obj_customer.IsAdmin = Convert.ToBoolean(cus.IsAdmin);
                }
            }
            return obj_customer;
        }

        public List<Customer> GetAllCustomer()
        {
            List<Customer> lst_Customer = new List<Customer>();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = (from a in db.tbl_Customers orderby a.CustomerName select a).ToList();
                foreach (var obj in data)
                {
                    Customer obj_customer = new Customer();
                    obj_customer.CustomerID = obj.CustomerID;
                    obj_customer.CustomerName = obj.CustomerName;
                    obj_customer.CustomerEmail = obj.CustomerEmail;
                    obj_customer.CustomerMobile = obj.CustomerMobile;
                    obj_customer.CustomerAddress = obj.CustomerAddress;
                    obj_customer.CustomerPassword = obj.CustomerPassword;
                    obj_customer.IsAdmin = obj.IsAdmin;
                    lst_Customer.Add(obj_customer);
                }
            }

            return lst_Customer;
        }
    }
}