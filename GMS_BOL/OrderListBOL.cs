using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GMS_BOL
{
    public class OrderListBOL
    {
        public List<SelectListItem> StatusList = new List<SelectListItem>();

        public List<OrderDetails> ListOrderDetails { get; set; }

        public string Status { get; set; }
        public string OrderID { get; set; }

        public class OrderDetails
        {
            public string OrderID { get; set; }
            public string Product { get; set; }
            public string Quantity { get; set; }
            public decimal TotalPrice { get; set; }
            public string DeliveryName { get; set; }
            public string address { get; set; }
            public string phoneNo { get; set; }
        }
    }
}
