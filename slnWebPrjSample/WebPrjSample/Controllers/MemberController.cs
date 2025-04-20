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
    }
}