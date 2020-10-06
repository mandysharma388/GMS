using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GMS_BOL
{
    public class Cart
    {
        public List<SelectListItem> PincodeList = new List<SelectListItem>();
        public List<SelectListItem> PlaceList = new List<SelectListItem>();

        public string firstname { get; set; }

        public string address { get; set; }        

        public string landmark { get; set; }

        public string pincode { get; set; }

        public string place { get; set; }

        public string phone_no { get; set; }        
    }
}
