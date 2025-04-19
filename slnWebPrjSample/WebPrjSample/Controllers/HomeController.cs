using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebPrjSample.Models;


namespace WebPrjSample.Controllers
{
    public class HomeController : Controller
    {
        WebPrjSampleEntities db = new WebPrjSampleEntities();
        public ActionResult Index()
        {
            var product = db.tProduct.OrderByDescending(m => m.fId).ToList();
            return View(product);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fUserId, string fPwd)
        {
            var member = db.tMember.Where(m => m.fUserId == fUserId && m.fPwd == fPwd)
                                   .FirstOrDefault();
            if (member == null)
            {
                ViewBag.Message = "帳號密碼錯誤，登入失敗";
                return View();
            }

            Session["Welcome"] = member.fName + "歡迎光臨";
            FormsAuthentication.RedirectFromLoginPage(fUserId, true);
            return RedirectToAction("Index", "Member");
        }
    }
}