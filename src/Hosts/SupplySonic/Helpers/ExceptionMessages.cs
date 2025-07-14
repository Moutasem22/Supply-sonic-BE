using Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public class ExceptionMessages : IExceptionMessages
{
    private readonly IStringLocalizer<SharedResource> _localizer;


    public ExceptionMessages(IStringLocalizer<SharedResource> Localizer)
    {
        _localizer = Localizer;
    }


    public void ReturnExceptionMessages(List<ErrorMessageDto> errorMessages)
    {
        errorMessages.Select(c =>
        {
            c.ErrorMessage = c.ErrorMessage != null || c.ErrorMessage != "" ? _localizer[c.ErrorMessage] : c.ErrorMessage;
            c.PropertyName = c.PropertyName != null || c.PropertyName != "" ? _localizer[c.PropertyName] : c.PropertyName;
            return c;
        }).ToList();

        if (errorMessages.Count > 0)
            throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(errorMessages));
    }

    public void ReturnExceptionMessages(ErrorMessageDto errorMessage)
    {
        this.ReturnExceptionMessages(new List<ErrorMessageDto> { errorMessage });
    }
}
