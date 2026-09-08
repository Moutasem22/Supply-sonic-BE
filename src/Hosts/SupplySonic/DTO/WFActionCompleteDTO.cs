using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WFActionCompleteDTO
    {
        public long? WFNextStateId { get; set; }
        public long AdminId { get; set; }
        public bool IsUnderStudy { get; set; }
        public long? WFCurrentStateId { get; set; }
        public long? WfInitStateId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompleted { get; set; }
    }
}
