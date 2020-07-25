using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class DeliManController
    {
        public List<DeliMan> GetAllDeliMan()
        {
            List<DeliMan> lst_DeliMan = new List<DeliMan>();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                var data = (from a in db.tbl_DeliMans select a).ToList();
                foreach (var obj in data)
                {
                    DeliMan obj_DeliMan = new DeliMan();
                    obj_DeliMan.DeliManID = obj.DeliManID;
                    obj_DeliMan.DeliMan_Name = obj.DeliMan_Name;
                    obj_DeliMan.DeliMan_Mobile = obj.DeliMan_Mobile;
                    obj_DeliMan.DeliMan_Email = obj.DeliMan_Email;
                    obj_DeliMan.DeliMan_NRC = obj.DeliMan_NRC;
                    obj_DeliMan.DeliMan_Address = obj.DeliMan_Address;
                    obj_DeliMan.DeliMan_Password = obj.DeliMan_Password;
                    obj_DeliMan.DeliGroupID = obj.DeliGroupID;

                    lst_DeliMan.Add(obj_DeliMan);
                }
            }

            return lst_DeliMan;
        }

        public DeliMan GetDeliManBy_Name_Mobile(string DeliManName, string DeliManMobile)
        {
            DeliMan obj_DeliMan = new DeliMan();
            using (OnlineShoppingDataContext db = new OnlineShoppingDataContext())
            {
                tbl_DeliMan deliMan = (from a in db.tbl_DeliMans where a.DeliMan_Name == DeliManName && a.DeliMan_Mobile == DeliManMobile select a).FirstOrDefault();

                if (deliMan != null)
                {
                    obj_DeliMan.DeliManID = deliMan.DeliManID;
                    obj_DeliMan.DeliMan_Name = deliMan.DeliMan_Name;
                    obj_DeliMan.DeliMan_Mobile = deliMan.DeliMan_Mobile;
                    obj_DeliMan.DeliMan_Email = deliMan.DeliMan_Email;
                    obj_DeliMan.DeliMan_NRC = deliMan.DeliMan_NRC;
                    obj_DeliMan.DeliMan_Address = deliMan.DeliMan_Address;
                    obj_DeliMan.DeliMan_Password = deliMan.DeliMan_Password;
                    obj_DeliMan.DeliGroupID = deliMan.DeliGroupID;
                }
            }
            return obj_DeliMan;
        }
    }
}