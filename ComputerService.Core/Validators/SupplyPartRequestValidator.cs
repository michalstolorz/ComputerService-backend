using ComputerService.Core.Dto.Request;
using ComputerService.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Validators
{
    public class SupplyPartRequestValidator : AbstractValidator<SupplyPartRequest>
    {
        public SupplyPartRequestValidator()
        {
            {
                RuleFor(x => x.PartId)
                    .GreaterThan(0)
                    .OnAnyFailure(x =>
                    {
                        throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.PartId)} is invalid.");
                    });

                RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .OnAnyFailure(x =>
                    {
                        throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Quantity)} is invalid.");
                    });
            }
        }
    }
}
