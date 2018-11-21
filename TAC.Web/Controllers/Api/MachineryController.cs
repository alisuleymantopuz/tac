using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TAC.Business;

namespace TAC.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineryController : Controller
    {
        private readonly IVehicleManager _vehicleManager;
        private static Random _random = new Random();

        public MachineryController(IVehicleManager vehicleManager)
        {
            _vehicleManager = vehicleManager;
        }

        [HttpPost("FakeStatusUpdate")]
        public async Task<IActionResult> Index(int? vehicleCount)
        {
            var requestedVehicleCount = vehicleCount ?? _random.Next(0, _vehicleManager.TotalVehicleCount());

            var randomVehicles = await _vehicleManager.PopulateRandomVehicle(requestedVehicleCount);

            if (randomVehicles.Any())
            {
                foreach (var vehicle in randomVehicles)
                {
                    _vehicleManager.UpdateLastConnection(vehicle.VIN, vehicle.RegistrationNumber).Wait();
                }
            }

            return Ok();
        }
    }
}