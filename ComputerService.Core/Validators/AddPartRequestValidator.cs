using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Validators
{
    public class AddPartRequestValidator : AbstractValidator<AddPartRequest>
    {
        public AddPartRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Name)} is invalid.");
                });
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Quantity)} is invalid.");
                });
            RuleFor(x => x.PartBoughtPrice)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.PartBoughtPrice)} is invalid.");
                });
        }
    }
}
