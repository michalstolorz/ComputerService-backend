using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(x => x)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
