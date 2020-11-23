using System;
using System.Collections.Generic;
using System.Text;
using ComputerService.Core.Exceptions;
using FluentValidation;

namespace ComputerService.Core.Validators
{
    public class ListStringsValidator : AbstractValidator<ICollection<string>>
    {
        public ListStringsValidator()
        {
            RuleForEach(x => x)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
