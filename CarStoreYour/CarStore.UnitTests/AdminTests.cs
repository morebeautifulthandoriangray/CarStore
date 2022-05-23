using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Controllers;
using CarStore.Domain.Abstract;
using CarStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
/*
namespace CarStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        /*
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Организация - создание имитированного хранилища данных
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1"},
                new Car { CarId = 2, Name = "Car2"},
                new Car { CarId = 3, Name = "Car3"},
                new Car { CarId = 4, Name = "Car4"},
                new Car { CarId = 5, Name = "Car5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Car> result = ((IEnumerable<Car>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Car1", result[0].Name);
            Assert.AreEqual("Car2", result[1].Name);
            Assert.AreEqual("Car3", result[2].Name);
        }
    }
}
        */
