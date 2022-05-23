using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Abstract
{
    public interface IShippingDetails
    {
        IEnumerable<IShippingDetails> ManyShippingDetails { get; }
    }
}
