using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            //cart.Lines().Count()
            if (cart.lineCollection.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);

                var person = storeDB.Orders.FirstOrDefault<ShippingDetails>((client) => client.Line1.Equals(shippingDetails.Line1));
                if(person == null)
                {
                    var newPerson = new ShippingDetails()
                    {
                        Line1 = shippingDetails.Line1,
                        Line2 = shippingDetails.Line2,
                        City = shippingDetails.City,
                        Country = shippingDetails.Country,
                        GiftWrap = shippingDetails.GiftWrap,
                        Name = shippingDetails.Name
                    };
                    storeDB.Orders.Add(newPerson);
                    storeDB.SaveChanges();
                }

                person = storeDB.Orders.FirstOrDefault<ShippingDetails>((client) => client.Line1.Equals(shippingDetails.Line1));

                foreach (CartLine cartline in cart.lineCollection)
                {
                    OrderLines order = new OrderLines()
                    {
                        OrderLineId = 1,
                        CarName = cartline.Car.CarId,
                        Quantity = cartline.Quantity,
                        PersonId = person.Line1
                    };
                    
                    storeDB.OrderLines.Add(order);

                    var car = storeDB.Cars.Find(cartline.Car.CarId);
                    car.Quantity -= cartline.Quantity;
                    storeDB.Entry(car).State = EntityState.Modified;
                }
                
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