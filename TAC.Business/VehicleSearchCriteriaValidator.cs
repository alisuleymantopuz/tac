using FluentValidation;
using FluentValidation.Results;

namespace TAC.Business
{
    public sealed class VehicleSearchCriteriaValidator : AbstractValidator<IVehicleSearchCriteria>
    {
        public VehicleSearchCriteriaValidator()
        {
        }

        public override ValidationResult Validate(ValidationContext<IVehicleSearchCriteria> context)
        {
            return (context.InstanceToValidate == null)
                    ? new ValidationResult(new[] { new ValidationFailure("VehicleSearchCriteriaInstance", "VehicleSearchCriteria instance can't be null!") })
                    : base.Validate(context);
        }
    }
}