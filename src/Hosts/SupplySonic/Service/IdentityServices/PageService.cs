using DB;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapster;
using DTO.IdentityDTO;
using DTO.CommandDTO;
using IServiceContractor.IdentityInterFaces;

namespace Service.IdentityServices
{
    public class PageService : IPageService
    {
        private DBContext _dbcontext;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public string lang { get; set; }

        public PageService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;

            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";
        }

        public ResultViewModel<List<PageDTO>> GetAll(QueryViewModel<PageDTO> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<PageDTO>>();

            var query = _dbcontext.Pages.Where(r => r.IsActive == true && r.IsDeleted != true).Include(x => x.PageActions).ThenInclude(x => x.Permissions).OrderByDescending(a => a.Id);

            var Total = query.Count();

            var pages = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            //List<PageResultDto> pageDtos = Mapper.Map<List<PageResultDto>>(pages);

            PagedDataResult.Data = pages.Adapt<List<PageDTO>>().ToList(); //Mapper.Map<List<PageDTO>>(pages).ToList();
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;


        }


        public ResultViewModel<List<PageCategoryDTO>> GetAllPageCategory(QueryViewModel<PageCategoryDTO> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<PageCategoryDTO>>();


            var query = _dbcontext.PageCategories.Where(r => r.IsActive == true && r.IsDeleted != true)
            //.OrderByDescending(a => a.Id)
            ;

            var Total = query.Count();

            var pages = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            //List<PageResultDto> pageDtos = Mapper.Map<List<PageResultDto>>(pages);

            PagedDataResult.Data = pages.Adapt<List<PageCategoryDTO>>();//Mapper.Map<List<PageCategoryDTO>>(pages).ToList();
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;


        }
        public ResultViewModel<PageDTO> EnablePageWF(int pageID)
        {
            return SetPageWFActivity(pageID, true);
        }


        public ResultViewModel<PageDTO> DisablePageWF(int pageID)
        {
            return SetPageWFActivity(pageID, false);
        }


        private ResultViewModel<PageDTO> SetPageWFActivity(int pageID, bool enableWF)
        {
            var result = new ResultViewModel<PageDTO>();

            var page = _dbcontext.Pages.FirstOrDefault(p => p.Id == pageID && p.IsActive && !p.IsDeleted && p.HasWorkflow == true);
            if (page == null)
                return new ResultViewModel<PageDTO>() { IsSuccess = false, Exception = "Invalid PageID" };

            if (enableWF)
                page.EnableWorkflow();
            else
                page.DisableWorkflow();

            _dbcontext.SaveChanges();
            result.Data = page.Adapt<PageDTO>(); //_mapper.Map<PageDTO>(page);
            result.IsSuccess = true;

            return result;
        }
    }
}
