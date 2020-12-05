using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    public class UpdateRepairDescriptionRequestValidator : AbstractValidator<UpdateRepairDescriptionRequest>
    {
        public UpdateRepairDescriptionRequestValidator()
        {
            RuleFor(x => x.RepairId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.RepairId)} is invalid.");
                });

            RuleFor(x => x.NewDescription)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.NewDescription)} is invalid.");
                });
        }
    }
}
