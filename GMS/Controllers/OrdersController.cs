using GMS_BLL;
using GMS_BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GMS_BOL.OrderListBOL;

namespace GMS.Controllers
{
    public class OrdersController : Controller
    {
        OrderListBLL orderbll = new OrderListBLL();
        public ActionResult OrderList()
        {
            OrderListBOL orderlist = new OrderListBOL();
            List<SelectListItem> StatusList = new List<SelectListItem>();

            StatusList = orderbll.ListStatus();
            orderlist.StatusList = StatusList;
            return View(orderlist);
        }

        [HttpPost]
        public ActionResult OrderList(OrderListBOL orderlist)
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();
            List<OrderDetails> ListOrderDetails = new List<OrderDetails>();
            if (ModelState.IsValid)
            {
                ListOrderDetails = orderbll.SearchList(orderlist);

            }

            StatusList = orderbll.ListStatus();
            orderlist.StatusList = StatusList;
            orderlist.ListOrderDetails = ListOrderDetails;
            return View(orderlist);
        }


    }
}