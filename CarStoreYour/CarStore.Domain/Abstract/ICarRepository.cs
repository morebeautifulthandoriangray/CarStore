using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities;

namespace CarStore.Domain.Abstract
{
    public interface ICarRepository
    {
        IEnumerable<Car> Cars { get; }
        void SaveCar(Car car);
        Car DeleteCar(int carId);
    }
}
