
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public interface IExceptionMessages
{
    void ReturnExceptionMessages(List<ErrorMessageDto> errorMessages);
    void ReturnExceptionMessages(ErrorMessageDto errorMessage);
}
