using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Validators
{
    public class UpdateUserInfoRequestValidator : AbstractValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.FirstName)} is invalid.");
                });

            RuleFor(x => x.LastName)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.LastName)} is invalid.");
                });

            RuleFor(x => x.Email)
                .EmailAddress()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Email)} is invalid.");
                });
            
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+[0-9]{1,3} ?\d{4,12}$")
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.PhoneNumber)} is invalid.");
                });
        }
    }
}
