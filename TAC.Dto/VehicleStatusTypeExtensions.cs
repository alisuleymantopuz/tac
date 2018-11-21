using TAC.Domain;

namespace TAC.Dto
{
    public static class VehicleStatusTypeExtensions
    {
        public static VehicleStatus ToVehicleStatus(this VehicleStatusType vehicleStatusType)
        {
            return ((VehicleStatus)vehicleStatusType);
        }

        public static VehicleStatus? ToVehicleStatus(this VehicleStatusType? vehicleStatusType)
        {
            if (vehicleStatusType == null)
            {
                return null;
            }

            return ((VehicleStatus)vehicleStatusType);
        }

        public static VehicleStatusType ToVehicleStatusType(this VehicleStatus vehicleStatus)
        {
            return ((VehicleStatusType)vehicleStatus);
        }

        public static VehicleStatusType? ToVehicleStatusType(this VehicleStatus? vehicleStatus)
        {
            if (vehicleStatus == null)
            {
                return null;
            }

            return ((VehicleStatusType)vehicleStatus);
        }
    }
}
