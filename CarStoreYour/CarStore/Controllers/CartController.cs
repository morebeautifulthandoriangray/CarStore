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
            var cartNew = new Cart();
            var arrayList = (List<CarForSession>)Session["list_cars"];
            foreach (CartLine cartLine in cart.lineCollection)
            {
                cartNew.lineCollection.Add(new CartLine()
                {
                    Car = cartLine.Car,
                    //ВОТ СЮДА ВЫТРЯХИВАТЬ ИЗ СЕССИИ КОЛИЧЕСТВО
                    Quantity = arrayList.Find(item => item.Id == cartLine.Car.CarId).Quantity,
                    QuantityInDB = storeDB.Cars.Find(cartLine.Car.CarId).Quantity
                });
            }
            var cartIndexViewModel = new CartIndexViewModel()
            {
                Cart = cartNew,
                ReturnUrl = returnUrl
            };
            return View(cartIndexViewModel);
        }

        public RedirectToRouteResult AddToCart(Cart cart, int carId, string returnUrl)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);

            if (car != null)
            {
                cart.AddItem(car, 1);
                /*CarForSession carForSession = new CarForSession()
                {
                    Id = car.CarId,
                    Quantity = 1
                };*/
                if (Session["list_cars"] == null)
                {
                    CarForSession carForSession = new CarForSession()
                    {
                        Id = car.CarId,
                        Quantity = 1
                    };
                    List<CarForSession> arrayProducts = new List<CarForSession>();
                    arrayProducts.Add(carForSession);
                    Session["list_cars"] = arrayProducts;
                }
                else
                {
                    var listCars = (List<CarForSession>)Session["list_cars"];
                    var carForSession = listCars.Find(item => item.Id == car.CarId);
                    if(carForSession == null)
                    {
                        carForSession = new CarForSession()
                        {
                            Id = car.CarId,
                            Quantity = 1
                        };
                    }
                    else
                    {
                        carForSession.Quantity = carForSession.Quantity + 1;
                    }
                    listCars.Add(carForSession);
                    Session["list_cars"] = listCars;
                }
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public ActionResult AddQuantityCarInSession(int id_car, int id_quantity, string returnUrl)
        {
            int quantity = id_quantity + 1;
            var carDB = storeDB.Cars.Find(id_car);
            if (quantity > carDB.Quantity) quantity = carDB.Quantity;
            var arrayList = (List<CarForSession>)Session["list_cars"];
            foreach (CarForSession car in arrayList)
            {
                if (car.Id == id_car)
                {
                    car.Quantity = quantity;
                }
            }
            Session["list_cars"] = arrayList;
            return RedirectToAction("Index", new { returnUrl});
        }

        [HttpPost]
        public ActionResult MinusQuantityCarInSession(int id_car, int id_quantity, string returnUrl)
        {
            int quantity = id_quantity - 1;
            if (quantity < 1) quantity = 1;
            var arrayList = (List<CarForSession>)Session["list_cars"];
            foreach (CarForSession car in arrayList)
            {
                if (car.Id == id_car)
                {
                    car.Quantity = quantity;
                }
            }
            Session["list_cars"] = arrayList;
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
            var arrayList = (List<CarForSession>)Session["list_cars"];
            Cart cartNew = new Cart();
            if(arrayList != null)
            {
                foreach (CarForSession carForSession in arrayList)
                {
                    CartLine cartLine = new CartLine()
                    {
                        Car = storeDB.Cars.Find(carForSession.Id),
                        Quantity = arrayList.Find(item => item.Id == carForSession.Id).Quantity
                    };
                    cartNew.lineCollection.Add(cartLine);
                }
            }
            return PartialView(cartNew);
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
            //if (ModelState.IsValid)
            //{
                orderProcessor.ProcessOrder(cart, shippingDetails);

                var person = (ShippingDetails)Session["user"];
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

                person = (ShippingDetails)Session["user"];
                var arrayList = (List<CarForSession>)Session["list_cars"];
                foreach (CartLine cartline in cart.lineCollection)
                {
                    OrderLines order = new OrderLines()
                    {
                        OrderLineId = 1,
                        CarName = cartline.Car.CarId,
                        Quantity = arrayList.Find(item => item.Id == cartline.Car.CarId).Quantity,
                        PersonId = person.Line1,
                        HistiryCarId = cartline.Car.CarId,
                        dateTime = DateTime.Now
                    };
                    
                    storeDB.OrderLines.Add(order);

                    var car = storeDB.Cars.Find(cartline.Car.CarId);
                    car.Quantity -= order.Quantity;
                    storeDB.Entry(car).State = EntityState.Modified;
                }
                
                storeDB.SaveChanges();
                cart.Clear();
                Session["list_cars"] = null;
                return View("Completed");
            //}
            //else
            //{
               // return View(shippingDetails);
            //}
        }

    }
}