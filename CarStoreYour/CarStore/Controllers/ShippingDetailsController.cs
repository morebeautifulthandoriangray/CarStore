using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarStore.Domain.Concrete;
using CarStore.Domain.Entities;

namespace CarStore.Controllers
{
    public class ShippingDetailsController : Controller
    {
        private EFDbContext db = new EFDbContext();
        // GET: ShippingDetails
        public ActionResult GetOrders()
        { 
            return View(from order in db.Orders select order);
        }
    }
}