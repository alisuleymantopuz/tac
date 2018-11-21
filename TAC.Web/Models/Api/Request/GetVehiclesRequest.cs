using TAC.Dto;

namespace TAC.Web.Models.Api.Request
{
    public class GetVehiclesRequest
    {
        public int? CustomerId { get; set; }
        public VehicleStatusType? VehicleStatusType { get; set; }
    }
}
