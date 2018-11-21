using System.Collections.Generic;
using System.Threading.Tasks;
using TAC.Dto;

namespace TAC.Business
{
    public interface IVehicleManager
    {
        Task<IList<VehicleInfo>> GetAllVehicles(IVehicleSearchCriteria vehicleSearchCriteria);
        Task<IList<CustomerInfo>> GetAllCustomers();
        Task<VehicleInfo> FindVehicle(string VIN, string registrationNumber);
        Task UpdateLastConnection(string VIN, string registrationNumber);
        Task VehichleDisconnect(string VIN, string registrationNumber);
        Task<IList<VehicleInfo>> GetUnavailableVehicles();
        Task<IList<VehicleInfo>> PopulateRandomVehicle(int recordCount);
        int TotalVehicleCount();
        Dictionary<string, string> GetVehicleStatuses();
    }
}
