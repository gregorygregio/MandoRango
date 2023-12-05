using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandoRango.Courier.Models
{
    public record struct CourierPosition(Guid CourierId, double Latitude, double Longitude)
    {
    }
}
