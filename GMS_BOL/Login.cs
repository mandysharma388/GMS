using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_BOL
{
    public class Login
    {
        public string phone_number { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
