using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    class WFCurrentRequestHistory
    {
        public long HistoryId { get; set; }
        public Guid WFRequestId { get; set; }
        public int LastActionType { get; set; }
        public int PageId { get; set; }
        public string PageCode { get; set; }
        public string PageNameEn { get; set; }
        public string PageNameAr { get; set; }
        public string UserName { get; set; }
        public string RequestStatus { get; set; }
        public string RoleNames { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
