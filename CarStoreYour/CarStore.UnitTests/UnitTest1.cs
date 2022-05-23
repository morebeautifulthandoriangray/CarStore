using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarStore.Controllers;
using CarStore.Domain.Abstract;
using CarStore.Domain.Entities;
using CarStore.HtmlHelpers;
using CarStore.Models;
using Moq;

namespace CarStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1"},
                new Car { CarId = 2, Name = "Car2"},
                new Car { CarId = 3, Name = "Car3"},
                new Car { CarId = 4, Name = "Car4"},
                new Car { CarId = 5, Name = "Car5"}
            });
            CarController controller = new CarController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            CarsListViewModel result = (CarsListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Car> games = result.Cars.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Car4");
            Assert.AreEqual(games[1].Name, "Car5");
        }



        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1"},
                new Car { CarId = 2, Name = "Car2"},
                new Car { CarId = 3, Name = "Car3"},
                new Car { CarId = 4, Name = "Car4"},
                new Car { CarId = 5, Name = "Car5"}
            });
            CarController controller = new CarController(mock.Object);
            controller.pageSize = 3;

            // Act
            CarsListViewModel result
                = (CarsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1", Category="Cat1"},
                new Car { CarId = 2, Name = "Car2", Category="Cat2"},
                new Car { CarId = 3, Name = "Car3", Category="Cat1"},
                new Car { CarId = 4, Name = "Car4", Category="Cat2"},
                new Car { CarId = 5, Name = "Car5", Category="Cat3"}
            });
            CarController controller = new CarController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Car> result = ((CarsListViewModel)controller.List("Cat2", 1).Model)
                .Cars.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1", Category="Fuel"},
                new Car { CarId = 2, Name = "Car2", Category="Electronic"},
                new Car { CarId = 3, Name = "Car3", Category="Fuel"},
                new Car { CarId = 4, Name = "Car4", Category="Fuel"},
                new Car { CarId = 5, Name = "Car5", Category="Electronic"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Fuel");
            Assert.AreEqual(results[1], "Electronic");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1", Category="Fuel"},
                new Car { CarId = 2, Name = "Car2", Category="Electronic"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Fuel";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns(new List<Car>
            {
                new Car { CarId = 1, Name = "Car1", Category="Cat1"},
                new Car { CarId = 2, Name = "Car2", Category="Cat2"},
                new Car { CarId = 3, Name = "Car3", Category="Cat1"},
                new Car { CarId = 4, Name = "Car4", Category="Cat2"}
            });
        
            CarController controller = new CarController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((CarsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((CarsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int resAll = ((CarsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(resAll, 5);
        }


    }
}
