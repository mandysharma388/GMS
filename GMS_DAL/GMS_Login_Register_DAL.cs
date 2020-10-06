using GMS_BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GMS_DAL
{
    public class GMS_Login_Register_DAL
    {
        GroMartDBEntities db = new GroMartDBEntities();
        public List<SelectListItem> FetchUserdata(Register Regdata)
        {
            DateTime DT = DateTime.Today;
            List<SelectListItem> SaveUserData = new List<SelectListItem>();
            TRN_USERDETAILS Userdetails = new TRN_USERDETAILS();
            var UserID = db.TRN_USERDETAILS.OrderByDescending(x => x.USER_ID).FirstOrDefault();
            if (UserID == null)
            {
                Userdetails.USER_ID = "U01" + DT.ToString("yy") + DT.AddYears(1).ToString("yy") + "00001";
            }
            else
            {
                Userdetails.USER_ID = "U01" + DT.ToString("yy") + DT.AddYears(1).ToString("yy") + (Convert.ToInt32(UserID.USER_ID.Substring(7, UserID.USER_ID.Length - 7)) + 1).ToString("D5");
            }
            Userdetails.FULL_NAME = Regdata.name;
            Userdetails.EMAIL_ID = Regdata.mailid;
            Userdetails.PHONE_NO = Regdata.phone;
            Userdetails.PASSWORD = Regdata.password;
            Userdetails.CREATED_BY = Regdata.name;
            Userdetails.CREATED_ON = DateTime.Now;
            Userdetails.UPDATED_BY = Regdata.name;
            Userdetails.UPDATED_ON = DateTime.Now;
            Userdetails.ISACTIVE = true;
            
            db.TRN_USERDETAILS.Add(Userdetails);
            db.SaveChanges();                       
            return SaveUserData;
        }

        public TRN_USERDETAILS checkuser(Login data)
        {
            var userdata = db.TRN_USERDETAILS.Where(x => x.PHONE_NO == data.phone_number && x.PASSWORD == data.password).FirstOrDefault();
            return userdata;
        }

        public List<SelectListItem> ListPincode()
        {
            List<SelectListItem> PincodeList = new List<SelectListItem>();
            List<MSTR_PINCODE> PincodeData = db.MSTR_PINCODE.Where(x => x.ISACTIVE == true).ToList();
            PincodeData.ForEach(x => PincodeList.Add(new SelectListItem { Text = x.PINCODE_NO, Value = x.PINCODE_ID }));
            return PincodeList;
        }

        public List<SelectListItem> ListPlace()
        {
            List<SelectListItem> PlaceList = new List<SelectListItem>();
            List<MSTR_PLACE> PlaceData = db.MSTR_PLACE.Where(x => x.ISACTIVE == true).ToList();
            PlaceData.ForEach(x => PlaceList.Add(new SelectListItem { Text = x.PLACE_NAME, Value = x.PLACE_ID }));
            return PlaceList;
        }

    }
}
