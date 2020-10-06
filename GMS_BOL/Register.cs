using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_BOL
{
   public  class Register
    {
        public string name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string mailid { get; set; }
        public string phone { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirm_password { get; set; }
    }
}
