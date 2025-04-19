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


    }
}