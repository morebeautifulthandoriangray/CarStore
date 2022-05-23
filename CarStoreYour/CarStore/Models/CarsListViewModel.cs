using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarStore.Domain.Entities;

namespace CarStore.Models
{
    public class CarsListViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}