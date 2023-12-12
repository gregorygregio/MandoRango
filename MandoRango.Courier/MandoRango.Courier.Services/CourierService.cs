using MandoRango.Courier.Data;
using MandoRango.Courier.Models;
using Microsoft.Extensions.Logging;

namespace MandoRango.Courier.Services
{
    public class CourierService
    {
        private readonly ILogger<CourierService> _logger;

        public CourierService(ILogger<CourierService> logger)
        {
            _logger = logger;
        }

        public async Task<MandoRango.Courier.Models.Courier> GetClosestCourier()
        {
            var _courierRepository = new CourierRepository("mongodb://172.17.0.2:27017","CourierDB");

            var availableCouriers = _courierRepository.GetAvailableCouriersByCity(3525904);

            throw new NotImplementedException();
        }
    }
}
