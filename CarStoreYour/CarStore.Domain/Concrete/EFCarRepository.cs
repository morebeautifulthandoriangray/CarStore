using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Abstract;
using CarStore.Domain.Entities;

namespace CarStore.Domain.Concrete
{
    public class EFCarRepository: ICarRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Car> Cars
        {
            get { return context.Cars; }
        }

        public void SaveCar(Car car)
        {
            if (car.CarId == 0) { 
                context.Cars.Add(car);
                HistoryCar historyCar = new HistoryCar()
                {
                    Id = 1,
                    Name = car.Name,
                    Description = car.Description,
                    Category = car.Category,
                    Price = car.Price
                };
                context.HistoryCars.Add(historyCar);
            }
            else
            {
                Car dbEntry = context.Cars.Find(car.CarId);
                if (dbEntry != null)
                {
                    HistoryCar historyCar = context.HistoryCars.Find(car.CarId);
                    dbEntry.Name = car.Name;
                    dbEntry.Description = car.Description;
                    dbEntry.Price = car.Price;
                    dbEntry.Category = car.Category;
                    dbEntry.ImageData = car.ImageData;
                    dbEntry.ImageMimeType = car.ImageMimeType;
                    dbEntry.Quantity = car.Quantity;

                    historyCar.Name = car.Name;
                    historyCar.Description = car.Description;
                    historyCar.Category = car.Description;
                    historyCar.Price = car.Price;

                    context.Entry(dbEntry).State = EntityState.Modified;
                    context.Entry(historyCar).State = EntityState.Modified;
                }
            }
            context.SaveChanges();
        }

        public Car DeleteCar(int carId)
        {
            Car dbEntry = context.Cars.Find(carId);
            if (dbEntry != null)
            {
                context.Cars.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
