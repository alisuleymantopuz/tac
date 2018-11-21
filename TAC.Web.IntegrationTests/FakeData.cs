using System.Collections.Generic;
using System.Linq;
using TAC.Domain;

namespace TAC.Web.IntegrationTests
{
    public static class FakeData
    {
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                 new Customer { Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje", },
                new Customer { Id = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm", },
                new Customer { Id = 3, Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala", }
            };
        }

        public static List<Vehicle> GetVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 2, VIN = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 3, VIN = "VLUR4X20009048066", RegistrationNumber = "GHI789", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 4, VIN = "YS2R4X20005388011", RegistrationNumber = "JKL012", CustomerId = 2, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 5, VIN = "YS2R4X20005387949", RegistrationNumber = "MNO345", CustomerId = 2, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 6, VIN = "VLUR4X20009048066", RegistrationNumber = "PQR678", CustomerId = 3, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 7, VIN = "YS2R4X20005387055", RegistrationNumber = "STU901", CustomerId = 3, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown }
            };
        }
        public static Customer GetCustomer
        {
            get
            {
                return GetCustomers().First();
            }
        }

        public static Vehicle GetVehicle
        {
            get
            {
                return GetVehicles().First();
            }
        }

    }
}
