using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WorkFlow:BaseEntity<int>
    {
        public string Name { get; private set; }
        public string NameAr { get; private set; }
        public EnumWFType WFType { get; private set; }
        public ICollection<WFTransition> WFTransitions { get; private set; }
        public ICollection<WFRequest> WFRequests { get; private set; }
        public WorkFlow()
        {
            WFTransitions = new HashSet<WFTransition>();
            WFRequests = new HashSet<WFRequest>();
        }
    }
}