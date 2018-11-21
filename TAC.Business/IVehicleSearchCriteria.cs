using TAC.Domain;

namespace TAC.Business
{
    public interface IVehicleSearchCriteria
    {
        int? CustomerId { get; set; }
        VehicleStatus? VehicleStatus { get; set; }
    }
}
