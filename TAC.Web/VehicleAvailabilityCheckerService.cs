using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TAC.Business;

namespace TAC.Web
{
    public class VehicleAvailabilityCheckerService : BackgroundService
    {
        private readonly IVehicleManager _vehicleManager;
        public VehicleAvailabilityCheckerService(IVehicleManager vehicleManager)
        {
            _vehicleManager = vehicleManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var unavailableVehicles = await _vehicleManager.GetUnavailableVehicles();

                if (unavailableVehicles.Any())
                {
                    foreach (var vehicle in unavailableVehicles)
                    {
                        _vehicleManager.VehichleDisconnect(vehicle.VIN, vehicle.RegistrationNumber).Wait();
                    }
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
