using Core.Enums;
using Core.WorkFlow;
using DB;
using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WorkFlowService : IWorkFlowService
    {
        DBContext _dbContext;
        IHttpContextAccessor _httpContextAccessor;
        //IWFRequestApprovalFactory _wFRequestApprovalFactory;
        //IWFRequestCompleteFactory _wFRequestCompleteFactory;
        //SysSettingsService _sysSettings;
        //ILogger _logger;


        public WorkFlowService(DBContext dbContext, IHttpContextAccessor httpContextAccessor)//, SysSettingsService sysSettings, IWFRequestCompleteFactory wFRequestCompleteFactory, IWFRequestApprovalFactory wFRequestApprovalFactory, ILogger<WorkFlowService> logger)
        {
            //this._logger = logger;
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
            //this._wFRequestApprovalFactory = wFRequestApprovalFactory;
            //this._wFRequestCompleteFactory = wFRequestCompleteFactory;
            //this._sysSettings = sysSettings;
        }
        public WFActionCompleteDTO ActionComplete(long? CurrentStateId, string ActionCode, Guid RequestId, WFRejectReason RejectReason = null, int? RequestCreatedBy = null, string entityName = null)
        {
            var result = new WFActionCompleteDTO();
            var CurrentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "UserId")?.Value ?? "0");
            var currentRequestAction = _dbContext.WFRequestAction.Include(x => x.WFAction).FirstOrDefault(x => x.IsReady == true
                && x.WFTransition.WFCurrentStateId == CurrentStateId //.WFCurrentState.Page.Code.ToLower() == pageCode.ToLower() 
                && x.WFAction.Action.Code.ToLower() == ActionCode.ToLower()
                && x.WFRequestId == RequestId);

            if (currentRequestAction == null)
                return result;

            var currentRequest = _dbContext.WFRequest.Include(x => x.WorkFlow).Include(x => x.WFCurrentState).ThenInclude(x => x.WFStateAssignedUsers).ThenInclude(x => x.WFStateUserStatus).FirstOrDefault(x => x.Id == RequestId);
            var currentRequestPostion = _dbContext.WFRequestPosition.FirstOrDefault(x => x.WFRequestId == RequestId);
            currentRequestAction.Complete();
            _dbContext.WFRequestAction.Update(currentRequestAction);
            var allRequestActions = _dbContext.WFRequestAction.Where(x => x.IsReady == true && x.WFTransition.WFCurrentStateId == CurrentStateId && x.WFRequestId == RequestId && x.WFTransitionId == currentRequestAction.WFTransitionId);
            allRequestActions.ToList().ForEach(x =>
            {
                x.Disable();
                _dbContext.WFRequestAction.Update(x);
            });

            var currntTransition = _dbContext.WFTransition.Include(x => x.WFCurrentState).Include(x => x.WFNextState).FirstOrDefault(x => x.Id == currentRequestAction.WFTransitionId);
            IQueryable<WFTransition> nextTranstions = null;
            if (currntTransition.WFNextStateId != null)
            {
                result.WFNextStateId = currntTransition.WFNextStateId;
                nextTranstions = _dbContext.WFTransition.Include(x => x.WFTransitionActions).Where(x => x.WorkFlowId == currentRequest.WorkFlowId && x.WFCurrentStateId == currntTransition.WFNextStateId);
                nextTranstions.SelectMany(x => x.WFTransitionActions, (parent, child) => new { TransId = parent.Id, WFActionId = child.WFActionId }).ToList().ForEach(x =>
                {
                    _dbContext.WFRequestAction.Add(new WFRequestAction(RequestId, x.TransId, x.WFActionId));
                });

                if (currntTransition.WFCurrentState.WFPageLevel == EnumWFLevel.FirstApproval)
                {
                    currentRequestPostion.FirstApproval = (ActionCode == WFActionsCode.Approve || ActionCode == WFActionsCode.Submit ? EnumWFRequestState.Approved : EnumWFRequestState.Rejected);
                }
                else if (currntTransition.WFCurrentState.WFPageLevel == EnumWFLevel.SecondApproval)
                {
                    currentRequestPostion.SecondApproval = (ActionCode == WFActionsCode.Approve || ActionCode == WFActionsCode.Submit ? EnumWFRequestState.Approved : EnumWFRequestState.Rejected);
                }
                else if (currntTransition.WFCurrentState.WFPageLevel == EnumWFLevel.ThirdApproval)
                {
                    currentRequestPostion.ThirdApproval = (ActionCode == WFActionsCode.Publish || ActionCode == WFActionsCode.Approve || ActionCode == WFActionsCode.Submit ? EnumWFRequestState.Approved : EnumWFRequestState.Rejected);
                }


                if (currntTransition.WFNextState.WFPageLevel == EnumWFLevel.FirstApproval)
                {
                    currentRequestPostion.FirstApproval = EnumWFRequestState.Pending;
                }
                else if (currntTransition.WFNextState.WFPageLevel == EnumWFLevel.SecondApproval)
                {
                    currentRequestPostion.SecondApproval = EnumWFRequestState.Pending;
                }
                else if (currntTransition.WFNextState.WFPageLevel == EnumWFLevel.ThirdApproval)
                {
                    currentRequestPostion.ThirdApproval = EnumWFRequestState.Pending;
                }

            }

            EnumWFRequestSituation FinalWFRequestSituation = currentRequest.WFRequestSituation;
            if (currentRequest.WFRequestSituation == EnumWFRequestSituation.New)
            {
                FinalWFRequestSituation = EnumWFRequestSituation.Send;
            }
            if (currentRequest.WFRequestSituation == EnumWFRequestSituation.Send && ActionCode == WFActionsCode.Approve)
            {
                FinalWFRequestSituation = EnumWFRequestSituation.UnderStudy;
                result.IsUnderStudy = true;
                result.WfInitStateId = _dbContext.WFTransition.FirstOrDefault(x => x.WorkFlowId == currentRequest.WorkFlowId && x.InitPoint == true)?.WFCurrentStateId;
            }

            if (currntTransition.WFNextState.Name.ToLower().IndexOf("ApproveRequest".ToLower()) > -1)
            {
                FinalWFRequestSituation = EnumWFRequestSituation.Approved;
                //_wFRequestApprovalFactory.Build(currentRequest.WorkFlow.WFType)?.Approval(currentRequest.Id);
                result.IsApproved = true;
                result.WfInitStateId = _dbContext.WFTransition.FirstOrDefault(x => x.WorkFlowId == currentRequest.WorkFlowId && x.InitPoint == true)?.WFCurrentStateId;
            }
            else if (currntTransition.WFNextState.Name.ToLower().IndexOf("CompleteRequest".ToLower()) > -1)
            {
                FinalWFRequestSituation = EnumWFRequestSituation.Completed;
                //_wFRequestCompleteFactory.Build(currentRequest.WorkFlow.WFType).Complete(currentRequest.Id);
                result.IsCompleted = true;
                result.WfInitStateId = _dbContext.WFTransition.FirstOrDefault(x => x.WorkFlowId == currentRequest.WorkFlowId && x.InitPoint == true)?.WFCurrentStateId;
            }
            else if (nextTranstions.FirstOrDefault().InitPoint)
            {
                FinalWFRequestSituation = EnumWFRequestSituation.RejectedToRequester;
            }

            EnumWFActionType actiontype;
            if (ActionCode == WFActionsCode.Submit)
                actiontype = EnumWFActionType.Submit;

            else if (ActionCode == WFActionsCode.Approve)
                actiontype = EnumWFActionType.Approve;

            else if (ActionCode == WFActionsCode.Reject)
                actiontype = EnumWFActionType.Reject;
            else if (ActionCode == WFActionsCode.Publish)
                actiontype = EnumWFActionType.Publish;
            else if (ActionCode == WFActionsCode.Unpublish)
                actiontype = EnumWFActionType.UnPublish;
            else
                actiontype = EnumWFActionType.Create;

            currentRequest.UpdateState(currntTransition.WFNextStateId ?? 0, currntTransition.WFCurrentStateId, actiontype, FinalWFRequestSituation, RejectReason);
            _dbContext.WFRequest.Update(currentRequest);
            result.AdminId = CurrentUserId;
            return result;
        }


        public WFRequest Init(Guid RequestId, EnumWFType WFType)
        {
            var WFRequestExist = _dbContext.WFRequest.FirstOrDefault(x => x.Id == RequestId);
            var CurrentWF = _dbContext.WorkFlow.FirstOrDefault(x => x.WFType == WFType && x.IsActive == true && x.IsDeleted != true);
            if (CurrentWF == null)
            {
                //throw new BusinessException(ExceptionType.RecordMissed, "There is no any workflow for that request type is available");
            }
            var initTransition = _dbContext.WFTransition.Include(x => x.WFTransitionActions).FirstOrDefault(x => x.InitPoint == true && x.WorkFlowId == CurrentWF.Id);
            initTransition.WFTransitionActions.ToList().ForEach(x =>
            {
                _dbContext.WFRequestAction.Add(new WFRequestAction(RequestId, x.WFTransitionId, x.WFActionId));
            });
            var wfRequest = WFRequestExist == null ? new WFRequest(RequestId, CurrentWF.Id) : WFRequestExist;
            wfRequest.UpdateState(initTransition.WFCurrentStateId, null, EnumWFActionType.Create, EnumWFRequestSituation.New, null);
            if (WFRequestExist == null)
            {
                _dbContext.WFRequest.Add(wfRequest);
                _dbContext.WFRequestPosition.Add(new WFRequestPosition() { WFRequestId = RequestId });
            }
            _dbContext.SaveChanges();
            return wfRequest;
        }

    }
}
