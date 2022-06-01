using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities
{
    public class OrderLines
    {
        [Key]
        public int OrderLineId { get; set; }
        public int? CarName { get; set; }
        public int Quantity { get; set; }
        public string PersonId { get; set; }
        public int HistiryCarId { get; set; }
        public DateTime dateTime { get; set; }
    }
}
