using GMS_BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static GMS_BOL.OrderListBOL;

namespace GMS_DAL
{
    public class OrderListDAL
    {
        GroMartDBEntities db = new GroMartDBEntities();
        public List<SelectListItem> ListStatus()
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();
            List<MSTR_DELIVERY_STATUS> StatusData = db.MSTR_DELIVERY_STATUS.Where(x => x.ISACTIVE == true).ToList();
            StatusData.ForEach(x => StatusList.Add(new SelectListItem { Text = x.STATUS_NAME, Value = x.STATUS_ID }));
            return StatusList;
        }

        public List<OrderDetails> SearchList(OrderListBOL orderlist)
        {
            List<OrderDetails> ListOrderDetails = new List<OrderDetails>();

            var Order = db.TRN_COUSTOMER_ORDER_DETAILS
                .Join(db.TRN_DELIVERY_ADDRESS_DETAILS,
                c => c.REF_DELIVERY_ID,
                o => o.DELIVERY_ID,
                (c, o) => new { c, o })
                .Join(db.TRN_ORDER_LOG,
                m => m.o.DELIVERY_ID,
                n => n.REF_DELIVERY_ID,
                (m, n) => new { m, n })
                .Where(x => x.m.c.ISACTIVE == true)
                .Where(x => (orderlist.Status != null && orderlist.Status != "") ? x.n.REF_STATUS_ID == orderlist.Status : true)
                .Where(x => (orderlist.OrderID != null && orderlist.OrderID != "") ? x.m.o.DELIVERY_ID == orderlist.OrderID : true)
                .Select(x => new
                {
                    OrderID = x.m.o.DELIVERY_ID,
                    ProductName = x.m.c.PRODUCT_NAME,
                    Qunatity = x.m.c.PRODUCT_QUANTITY,
                    Total = x.m.c.TOTAL_PRICE,
                    DeliveryName = x.m.o.NAME,
                    DeliveryAddress = x.m.o.DELIVERY_ADDRESS,
                    PhoneNo = x.m.o.MOBILE_NO
                }).OrderBy(x => x.OrderID).ToList();

            foreach(var item in Order)
            {
                OrderDetails details = new OrderDetails();

                details.OrderID = item.OrderID;
                details.Quantity = item.Qunatity;
                details.Product = item.ProductName;
                details.TotalPrice = Convert.ToDecimal(item.Total);
                details.DeliveryName = item.DeliveryName;
                details.address = item.DeliveryAddress;
                details.phoneNo = item.PhoneNo;

                ListOrderDetails.Add(details);
            }
            return ListOrderDetails;
        }
    }
}
