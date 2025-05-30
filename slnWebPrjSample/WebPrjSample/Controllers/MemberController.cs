﻿using System;
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

        [HttpPost]
        public ActionResult ShoppingCar(string fReceiver, string fPhone, string fEmail, string fAddress)
        {
            string fUserId = User.Identity.Name;
            string guid = Guid.NewGuid().ToString();

            tOrder order = new tOrder();
            order.fOrderGuid = guid;
            order.fUserId = fUserId;
            order.fReceiver = fReceiver;
            order.fPhone = fPhone;
            order.fEmail = fEmail;
            order.fAddress = fAddress;
            order.fDate = DateTime.Now;
            db.tOrder.Add(order);

            var carList = db.tOrderDetail.Where(m => m.fIsApproved == "否" && m.fUserId == fUserId).ToList();
            foreach(var item in carList)
            {
                item.fOrderGuid = guid;
                item.fIsApproved = "是";
            }

            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult AddCar(string fPId)
        {
            string fUserId = User.Identity.Name;
            var currentCar = db.tOrderDetail.Where(m => m.fPId == fPId && m.fIsApproved == "否"
                                                                       && m.fUserId == fUserId).FirstOrDefault();
            if (currentCar == null)
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

        public ActionResult OrderList()
        {
            string fUserId = User.Identity.Name;

            var orders = db.tOrder.Where(m => m.fUserId == fUserId).OrderByDescending(m => m.fDate).ToList();

            return View(orders);
        }

        public ActionResult OrderDetail(string fOrderGuid)
        {
            var orderDetails = db.tOrderDetail.Where(m => m.fOrderGuid == fOrderGuid).ToList();
            return View(orderDetails);
        }
    }
}