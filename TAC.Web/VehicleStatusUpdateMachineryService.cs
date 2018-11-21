using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TAC.Business;

namespace TAC.Web
{
    public class VehicleStatusUpdateMachineryService : BackgroundService
    {
        private readonly IVehicleManager _vehicleManager;
        private static Random _random = new Random();
        public VehicleStatusUpdateMachineryService(IVehicleManager vehicleManager)
        {
            _vehicleManager = vehicleManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var totalVehicleCount = _vehicleManager.TotalVehicleCount();

                var randomVehicles = await _vehicleManager.PopulateRandomVehicle(_random.Next(0, totalVehicleCount));

                if (randomVehicles.Any())
                {
                    foreach (var vehicle in randomVehicles)
                    {
                        _vehicleManager.UpdateLastConnection(vehicle.VIN, vehicle.RegistrationNumber).Wait();
                    }
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
