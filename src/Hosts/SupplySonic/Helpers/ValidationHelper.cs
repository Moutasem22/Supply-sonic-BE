using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public class ValidationHelper
{
    private readonly IExceptionMessages _exceptionMessages;
    public ValidationHelper(IExceptionMessages exceptionMessages)
    {
        _exceptionMessages = exceptionMessages;
    }
    public void Validate<T>(T dto, IValidator<T> validator)
    {
        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => new ErrorMessageDto() { PropertyName = x.PropertyName, ErrorMessage = x.ErrorMessage }).ToList();

            if (errorMessages.Count > 0)
                _exceptionMessages.ReturnExceptionMessages(errorMessages);
        }

    }
}
