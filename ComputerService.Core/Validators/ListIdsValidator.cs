using ComputerService.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Validators
{
    public class ListIdsValidator : AbstractValidator<ICollection<int>>
    {
        public ListIdsValidator()
        {
            RuleForEach(x => x)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
