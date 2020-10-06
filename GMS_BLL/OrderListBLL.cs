using GMS_BOL;
using GMS_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static GMS_BOL.OrderListBOL;

namespace GMS_BLL
{
    public class OrderListBLL
    {
        OrderListDAL orderdal = new OrderListDAL();
        public List<SelectListItem> ListStatus()
        {
            List<SelectListItem> StatusList = orderdal.ListStatus();
            return StatusList;
        }

        public List<OrderDetails> SearchList(OrderListBOL orderlist)
        {
            List<OrderDetails> ListOrderDetails = orderdal.SearchList(orderlist);
            return ListOrderDetails;
        }
    }
}
