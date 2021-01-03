using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    public class UpdateRepairUsedPartsRequestValidator : AbstractValidator<UpdateRepairUsedPartsRequest>
    {
        public UpdateRepairUsedPartsRequestValidator()
        {
            RuleFor(x => x.RepairId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });

            RuleFor(x => x.PartId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });

            RuleFor(x => x.UsedPartQuantity)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
