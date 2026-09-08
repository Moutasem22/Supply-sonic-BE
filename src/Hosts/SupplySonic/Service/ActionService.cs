using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Action = Core.Models.Identity.Action;
using Mapster;
namespace Service
{
    public class ActionService:IActionService
    {
        private DBContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string lang { get; set; }

        public ActionService(DBContext dbcontext,  IHttpContextAccessor httpContextAccessor)
        {  
            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";
            this._dbcontext = dbcontext;
        }

        public ResultViewModel<List<ActionDTO>> GetAll(QueryViewModel<ActionDTO> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<ActionDTO>>();

            try
            {
                var query = _dbcontext.Actions.Where(r => r.IsActive == true && r.IsDeleted != true && r.IsMaster!=true).OrderByDescending(a => a.Id);

                var Total = query.Count();

                var actions = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

                //List<ActionResultDto> actionDtos = Mapper.Map<List<ActionResultDto>>(actions);

                PagedDataResult.Data = actions.Adapt<List<ActionDTO>>().ToList();// Mapper.Map<List<ActionDTO>>(actions).ToList();
                PagedDataResult.PageSize = queryViewModel.PageSize;
                PagedDataResult.PageNumber = queryViewModel.PageNumber;
                PagedDataResult.Total = Total;
                PagedDataResult.IsSuccess = true;
                return PagedDataResult;
            }
            catch
            {
                
                return PagedDataResult;
            }            

        }
    }
}
