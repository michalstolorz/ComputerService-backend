using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.OldPassword)} is invalid.");
                });

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .NotEqual(x => x.OldPassword)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.NewPassword)} is invalid.");
                });

            RuleFor(x => x.NewPasswordConfirmation)
                .NotEmpty()
                .Equal(x => x.NewPassword)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.NewPasswordConfirmation)} is invalid.");
                });
        }
    }
}
