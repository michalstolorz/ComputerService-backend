using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;


namespace ComputerService.Core.Validators
{
    public class EvaluateRepairCostRequestValidator : AbstractValidator<EvaluateRepairCostRequest>
    {
        public EvaluateRepairCostRequestValidator()
        {
            RuleFor(x => x.RepairId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.RepairId)} is invalid.");
                });

            RuleFor(x => x.RepairCost)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.RepairCost)} is invalid.");
                });
        }
    }
}
