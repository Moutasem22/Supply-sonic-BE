using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums;

public enum EnumRequestStatus
{
    NotStarted = 1,
    InProgress = 2,
    Complated = 3,
    CancelFromProvider = 4,
    CancelFromAdmin = 5
}
