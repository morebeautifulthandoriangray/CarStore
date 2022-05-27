using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Domain.Entities;

namespace CarStore.Domain.Concrete
{
    public class EFDbContext: DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<ShippingDetails> Orders { get; set; }

        public DbSet<HistoryCar> HistoryCars { get; set; }

        public DbSet<OrderLines> OrderLines { get; set; }
        
    }
}
