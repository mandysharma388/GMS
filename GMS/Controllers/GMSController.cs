using GMS_BLL;
using GMS_BOL;
using GMS_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GMS.Controllers
{
    public class GMSController : Controller
    {
        GMS_Login_Register_BLL GMSBLL = new GMS_Login_Register_BLL();
        GroMartDBEntities db = new GroMartDBEntities();
        // GET: GMS
        public ActionResult Login()
        {
            Login data = new Login();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login data)
        {
            var userdata = GMSBLL.checkuser(data);
            if(userdata == null)
            {
                TempData["Message"] = "Wrong Phone No or Password";
                return View("Login", userdata);
            }

            else
            {
                Session["USERID"] = userdata.USER_ID;
                Session["USERNAME"] = userdata.FULL_NAME;
                Session["ROLLID"] = userdata.REF_ROLE_ID;
                return RedirectToAction("Dashboard");
            }            
        }

        public ActionResult Register()
        {
            Register Regdata = new Register();
            return View(Regdata);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register Regdata)
        {
            TRN_USERDETAILS Userdetails = new TRN_USERDETAILS();
            List<TRN_USERDETAILS> phoneno = db.TRN_USERDETAILS.Where(x=>x.PHONE_NO == Regdata.phone).ToList();

            if (ModelState.IsValid)
            {
                if(phoneno.Count > 0)
                {
                    TempData["Message"] = "Mobile No alreday exist";
                }
                else
                {
                    List<SelectListItem> SaveUserData = GMSBLL.FetchUserdata(Regdata);
                    return RedirectToAction("Login");
                }                
            }
            else
            {

            }
            return View(Regdata);
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public JsonResult GetProductList()
        {            
            db.Configuration.ProxyCreationEnabled = false;
            var product = db.MSTR_PRODUCT.ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cart()
        {
            Cart cart = new Cart();
            
            List<SelectListItem> PincodeList = GMSBLL.ListPincode();
            List<SelectListItem> PlaceList = GMSBLL.ListPlace();

            cart.PincodeList = PincodeList;
            cart.PlaceList = PlaceList;
            return View(cart);
        }
        public JsonResult Productdetails(string item)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var product = db.MSTR_PRODUCT.Where(x => x.PRODUCT_ID == item).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public JsonResult cart(Cart cart,List<PurchaseItem> item)
        {
            var USERNAME = Session["USERNAME"].ToString();

            List<SelectListItem> PincodeList = GMSBLL.ListPincode();
            List<SelectListItem> PlaceList = GMSBLL.ListPlace();
            var DT = DateTime.Today;
            cart.PincodeList = PincodeList;
            cart.PlaceList = PlaceList;
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                TRN_DELIVERY_ADDRESS_DETAILS address = new TRN_DELIVERY_ADDRESS_DETAILS();
                var DeliveryID = db.TRN_DELIVERY_ADDRESS_DETAILS.OrderByDescending(x => x.DELIVERY_ID).FirstOrDefault();
                if (DeliveryID == null)
                {
                    address.DELIVERY_ID = "D01" + DT.ToString("yy") + DT.AddYears(1).ToString("yy") + "000001";
                }
                else
                {
                    address.DELIVERY_ID = "D01" + DT.ToString("yy") + DT.AddYears(1).ToString("yy") + (Convert.ToInt32(DeliveryID.DELIVERY_ID.Substring(7, DeliveryID.DELIVERY_ID.Length - 7)) + 1).ToString("D6");
                }
                
                address.NAME = cart.firstname;
                address.DELIVERY_ADDRESS = cart.address;
                address.LANDMARK = cart.landmark;
                address.MOBILE_NO = cart.phone_no;
                address.REF_PINCODE_ID = cart.pincode;
                address.REF_PLACE_ID = cart.place;
                address.CREATED_ON = DateTime.Now;
                address.CREATED_BY = USERNAME;
                address.ISACTIVE = true;
                db.TRN_DELIVERY_ADDRESS_DETAILS.Add(address);
                var Ref_Delivery_ID = address.DELIVERY_ID;

                TRN_ORDER_LOG log = new TRN_ORDER_LOG();
                log.ORDER_LOG_ID = Guid.NewGuid().ToString();
                log.REF_DELIVERY_ID = Ref_Delivery_ID;
                log.REF_STATUS_ID = "0001";
                log.CREATED_BY = USERNAME;
                log.CREATED_ON = DateTime.Now;
                log.ISACTIVE = true;
                db.TRN_ORDER_LOG.Add(log);

                List<TRN_COUSTOMER_ORDER_DETAILS> cartvalue = new List<TRN_COUSTOMER_ORDER_DETAILS>();
                MSTR_PRODUCT product_price = new MSTR_PRODUCT();

                foreach (var itm in item)
                {
                    TRN_COUSTOMER_ORDER_DETAILS cartval = new TRN_COUSTOMER_ORDER_DETAILS();
                    var product_details = db.MSTR_PRODUCT.Where(x => x.PRODUCT_ID == itm.product).FirstOrDefault();
                    cartval.ORDER_ID = Guid.NewGuid().ToString();
                    cartval.PRODUCT_NAME = product_details.PRODUCT_NAME;
                    cartval.PRODUCT_QUANTITY = itm.count;
                    cartval.PRODUCT_PRICE = product_details.PRODUCT_PRICE;
                    cartval.TOTAL_PRICE = (Convert.ToInt32(itm.count) * Convert.ToInt32(product_details.PRODUCT_PRICE)).ToString();
                    cartval.REF_DELIVERY_ID = Ref_Delivery_ID;                    
                    cartval.CREATED_ON = DateTime.Now;
                    cartval.CREATED_BY = USERNAME;
                    cartval.ISACTIVE = true;
                    cartvalue.Add(cartval);                    
                }
                if (cartvalue.Count > 0)
                {
                    db.TRN_COUSTOMER_ORDER_DETAILS.AddRange(cartvalue);
                }
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}