using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Enums
{
    public enum EnumWFRequestSituation
    {
        New=0,
        UnderStudy=1,
        RejectedToRequester=2, // rejected and returned to requester
        Approved=3, // Final approved from last workflow 
        Completed=4, // License completed 
        IsNull = 5,
        Send=6//تم الارسال من مقدم الطلب
    }
}
