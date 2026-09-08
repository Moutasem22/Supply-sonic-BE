using Core.Enums;
using Core.WorkFlow;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor
{
    public interface IWorkFlowService
    {
        WFRequest Init(Guid RequestId, EnumWFType WFType);
        WFActionCompleteDTO ActionComplete(long? CurrentStateId, string ActionCode, Guid RequestId, WFRejectReason RejectReason = null, int? RequestCreatedBy = null, string entityName = null);
    }
}
