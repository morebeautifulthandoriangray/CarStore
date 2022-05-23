using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarStore.Domain.Abstract;

namespace CarStore.Controllers
{
    public class NavController : Controller
    {
        private ICarRepository repository;

        public NavController(ICarRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Cars
                .Select(car => car.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }

}