using TAC.Domain;

namespace TAC.Business
{
    public class VehicleSearchCriteria : IVehicleSearchCriteria
    {
        public int? CustomerId { get; set; }
        public VehicleStatus? VehicleStatus { get; set; }
    }
}
