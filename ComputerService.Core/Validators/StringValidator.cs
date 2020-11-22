using ComputerService.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Validators
{
    public class StringValidator : AbstractValidator<string>
    {
        public StringValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
