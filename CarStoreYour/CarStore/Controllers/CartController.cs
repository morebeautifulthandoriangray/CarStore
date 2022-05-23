using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarStore.Domain.Abstract;
using CarStore.Domain.Concrete;
using CarStore.Domain.Entities;
using CarStore.Models;

namespace CarStore.Controllers
{
    public class CartController : Controller
    {
        public EFDbContext storeDB = new EFDbContext();
        private ICarRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(ICarRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int carId, string returnUrl)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);

            if (car != null)
            {
                cart.AddItem(car, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int carId, string returnUrl)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);

            if (car != null)
            {
                cart.RemoveLine(car);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {

            //var order = new ShippingDetails();
            var carOrder = new CartLine();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);

                foreach (CartLine cartline in cart.lineCollection)
                {
                    ShippingDetails order = new ShippingDetails();
                    order.Name = shippingDetails.Name;
                    order.Line1 = shippingDetails.Line1;
                    order.Line2 = shippingDetails.Line2;
                    order.City = shippingDetails.City;
                    order.Country = shippingDetails.Country;
                    order.CarName = cartline.Car.CarId;
                    order.Quantity = cartline.Quantity;
                    storeDB.Orders.Add(order);

                }
                /*
                order.Name = shippingDetails.Name;
                order.Line1 = shippingDetails.Line1;
                order.Line2 = shippingDetails.Line2;
                order.City = shippingDetails.City;
                order.Country = shippingDetails.Country;*/
                // order.CarName = lineCollection


                //storeDB.Orders.Add(order);
                

                storeDB.SaveChanges();

                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}