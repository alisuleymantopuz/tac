using System;

namespace TAC.Dto
{
    public class VehicleInfo
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? ConnectedOn { get; set; }
        public DateTime? LastConnectedOn { get; set; }
        public VehicleStatusType? VehicleStatus { get; set; }
        public int? CustomerId { get; set; }
        public CustomerInfo Customer { get; set; }
        public string VehicleStatusName
        {
            get
            {
                return VehicleStatus != null ? VehicleStatus.ToString() : string.Empty;
            }
        }
        public string LastConnectedOnDateTimeString
        {
            get
            {
                return LastConnectedOn.HasValue ? LastConnectedOn.Value.ToString("MMM ddd d HH:mm yyyy") : string.Empty;
            }
        }
        public string ConnectedOnDateTimeString
        {
            get
            {
                return ConnectedOn.HasValue ? ConnectedOn.Value.ToString("MMM ddd d HH:mm yyyy") : string.Empty;
            }
        }
    }
}
