using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAC.Domain
{
    public class Vehicle : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string VIN { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? LastConnectedOn { get; set; }
        public DateTime? ConnectedOn { get; set; }
        public VehicleStatus VehicleStatus { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
