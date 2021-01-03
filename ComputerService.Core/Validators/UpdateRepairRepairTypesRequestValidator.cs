using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    public class UpdateRepairRepairTypesRequestValidator : AbstractValidator<UpdateRepairRepairTypesRequest>
    {
        public UpdateRepairRepairTypesRequestValidator()
        {
            RuleFor(x => x.RepairId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });

            RuleForEach(x => x.RepairTypeIds)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
