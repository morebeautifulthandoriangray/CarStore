using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CarStore.Domain.Abstract;
using CarStore.Domain.Concrete;
using CarStore.Domain.Entities;
using CarStore.Models;

namespace CarStore.Controllers
{
    public class AdminController : Controller
    {
        ICarRepository repository;

        

        public AdminController(ICarRepository repo)
        {
            repository = repo;
        }

       

        
        [HttpGet]
        public ViewResult Index()
        {
            return View(repository.Cars);
        }

        


        public ViewResult Edit(int carId)
        {
            Car car = repository.Cars
                .FirstOrDefault(g => g.CarId == carId);
            return View(car);
        }

        [HttpPost]
        public ActionResult Edit(Car car, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    car.ImageMimeType = image.ContentType;
                    car.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(car.ImageData, 0, image.ContentLength);
                }
                repository.SaveCar(car);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", car.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(car);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Car());
        }

        [HttpPost]
        public ActionResult Delete(int carId)
        {
            Car deletedCar = repository.DeleteCar(carId);
            if (deletedCar != null)
            {
                TempData["message"] = string.Format("Машина \"{0}\" была удалена",
                    deletedCar.Name);
            }
            return RedirectToAction("Index");
        }

        private EFDbContext db = new EFDbContext();
        // GET: ShippingDetails
        public ActionResult Orders()
        {
            return View(from order in db.Orders select order);
        }

        private IQueryable<CarDTO> SelectCustomersWithrders(string personId)
        {
            var orderForPerson = from o in db.OrderLines
                                 join c in db.HistoryCars on o.HistiryCarId equals c.Id into temp1
                                 from t1 in temp1.DefaultIfEmpty()
                                 where o.PersonId == personId
                                 select new CarDTO
                                 {
                                     CarId = o.HistiryCarId,
                                     Category = t1.Category,
                                     Description = t1.Description,
                                     Name = t1.Name,
                                     Price = t1.Price,
                                     Quantity = o.Quantity,
                                     dateTime = o.dateTime
                                 };
            return orderForPerson;
        }

        public ActionResult Details(string personId)
        {
            return View(SelectCustomersWithrders(personId));
        }

    }
}
