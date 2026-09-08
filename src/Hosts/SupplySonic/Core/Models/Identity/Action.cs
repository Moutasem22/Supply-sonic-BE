using Core.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity
{
    public class Action:BaseEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public bool? IsMaster { get; set; }
        public ICollection<PageAction> PageActions { get; set; }
        public WFAction WFAction { get; set; }
        public Action()
        {
            PageActions = new HashSet<PageAction>();
        }
    }
}
