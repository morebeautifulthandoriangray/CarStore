using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities
{
    public class Cart
    {
        public List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Car car, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Car.CarId == car.CarId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Car = car,
                    Quantity = quantity
                });
            }
            else
            {
                lineCollection.Remove(line);
                line.Quantity += quantity;
                lineCollection.Add(line);
            }
        }

        public void RemoveLine(Car car)
        {
            lineCollection.RemoveAll(l => l.Car.CarId == car.CarId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Car.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        
    }

    public class CartLine
    {
        public Car Car { get; set; }
        public int Quantity { get; set; }
        public int? QuantityInDB { get; set; }
    }
}
