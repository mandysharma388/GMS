using GMS_BOL;
using GMS_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GMS_BLL
{    
    public class GMS_Login_Register_BLL
    {
        GMS_Login_Register_DAL GMSDAL = new GMS_Login_Register_DAL();

        public List<SelectListItem> FetchUserdata(Register Regdata)
        {
            List<SelectListItem> SaveUserData = GMSDAL.FetchUserdata(Regdata);
            return SaveUserData;
        }

        public TRN_USERDETAILS checkuser(Login data)
        {
            var userdata = GMSDAL.checkuser(data);
            return userdata;
        }

        public List<SelectListItem> ListPincode()
        {
            List<SelectListItem> PincodeList = GMSDAL.ListPincode();
            return PincodeList;
        }

        public List<SelectListItem> ListPlace()
        {
            List<SelectListItem> PlaceList = GMSDAL.ListPlace();
            return PlaceList;
        }
    }
}
