using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;
using WebPrjSample.Models;
using System.Web.Security;

namespace WebPrjSample.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        WebPrjSampleEntities db = new WebPrjSampleEntities();
        // GET: Member
        public ActionResult Index()
        {
            var products = db.tProduct.OrderByDescending(m => m.fId).ToList();
            return View("../Home/Index","_LayoutMember",products);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ShoppingCar()
        {
            string fUserUId = User.Identity.Name;
            var orderDetails = db.tOrderDetail.Where(m => m.fUserId == fUserUId && m.fIsApproved == "否").ToList();
            return View(orderDetails);
        }

        public ActionResult AddCar(string fPId)
        {
            string fUserId = User.Identity.Name;
            var currentCar = db.tOrderDetail.Where(m => m.fPId == fPId && m.fIsApproved == "否"
                                                                       && m.fUserId == fUserId).FirstOrDefault();
            if (currentCar != null)
            {

                var product = db.tProduct.Where(m => m.fPId == fPId).FirstOrDefault();

                tOrderDetail orderDetail = new tOrderDetail();
                orderDetail.fUserId = fUserId;
                orderDetail.fPId = product.fPId;
                orderDetail.fName = product.fName;
                orderDetail.fPrice = product.fPrice;
                orderDetail.fQty = 1;
                orderDetail.fIsApproved = "否";
                db.tOrderDetail.Add(orderDetail);
            }
            else
            {
                currentCar.fQty += 1;
            }
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        public ActionResult DeleteCar(int fId)
        {
            var orderDetail = db.tOrderDetail.Where(m => m.fId == fId).FirstOrDefault();
            db.tOrderDetail.Remove(orderDetail);
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }
    }
}