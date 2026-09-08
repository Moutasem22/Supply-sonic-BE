using DTO;
using Core.Models.Identity;
using DB;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IServiceContractor;
using Microsoft.AspNetCore.SignalR;
using Service.HubConfig;
using Mapster;
using Microsoft.Extensions.Hosting;
using Core.Models;
using Core.Enums;
using IServiceContractor.ICommonService;
using FluentValidation;
using System.Data;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using AspNetCore.Reporting;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Service
{
    public class UserService : IUserService
    {
        private DBContext _dbcontext;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _env;
        private IHubContext<NotificationHub> _hub;
        private readonly IBaseService _baseService;
        private readonly IValidator<UserAddEditDto> _validator;
        private readonly IValidator<UserProfileEditDto> _profileValidator;
        private IConfiguration _Configuration;
  
        private readonly INotificationService _notificationService;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public UserService(IBaseService baseService, IValidator<UserAddEditDto> validator, IValidator<UserProfileEditDto> profileValidator, IOptions<AppSettings> options,
            IConfiguration configuration, IHostEnvironment env, IHubContext<NotificationHub> hub, INotificationService notificationService, IPasswordHasher<AppUser> passwordHasher)
        {
            this._baseService = baseService;

            this._validator = validator;
            this._profileValidator = profileValidator;
            _dbcontext = _baseService.Context;
            _appSettings = options.Value;
            _httpContextAccessor = baseService.HttpContextAccessor;
            _env = env;
            _hub = hub;
            _Configuration = configuration;

            _passwordHasher = passwordHasher;
            this._notificationService = notificationService;
            var url = _httpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "";
            TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
            .Map(dest => dest.FilePath, src => url + src.Path);

            TypeAdapterConfig<AppUser, UserResultDto>.NewConfig()
            //.Map(dest => dest.IsPMO, src => (src.UserType & (int)EnumUserType.PMO) > 0)
            //.Map(dest => dest.IsLicensedUser, src => (src.UserType & (int)EnumUserType.LicensedUser) > 0)
            //.Map(dest => dest.IsSystemAdmin, src => (src.UserType & (int)EnumUserType.SystemAdmin) > 0)
            .Map(dest => dest.IsExternal, src => src.UserCategory == EnumUserCategory.ExternalUser)
            //.Map(des => des.IsInitiativesManager, src => (src.UserType & (int)EnumUserType.IsInitiativesManager) > 0)
            //.Map(des => des.IsStrategyManager, src => (src.UserType & (int)EnumUserType.IsStrategyManager) > 0)
            .Map(dest => dest.UserRoleNames, src => src.UserRoles != null ? string.Join(", ", (src.UserRoles.Select(x => x.Role != null ? (baseService.Language == "ar" ? x.Role.NameAr : x.Role.Name) : "").ToArray())) : "")

            .Map(dest => dest.Birthdate, src => src.Birthdate != null ? src.Birthdate.Value.ToISOString() : null)
            ;

        }


        public async Task<ResultViewModel<List<UserResultDto>>> GetAll(QueryViewModel<UserResultDto> queryViewModel)
        {
            IQueryable<AppUser> query = _baseService.Context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(s => s.ProfileAttachment)
                .Where(r => r.IsDeleted != true);

            return await GetAllUsersData(queryViewModel, query);
        }

        public async Task<ResultViewModel<List<UserResultDto>>> GetAllActiveUsers(QueryViewModel<UserResultDto> queryViewModel)
        {
            IQueryable<AppUser> query = _baseService.Context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .Where(r => r.IsActive && r.IsDeleted != true);

            return await GetAllUsersData(queryViewModel, query);

        }

        public async Task<ResultViewModel<List<UserResultDto>>> GetAllDeletedUser(QueryViewModel<UserResultDto> queryViewModel)
        {
            IQueryable<AppUser> query = _baseService.Context.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .Where(r => r.IsActive && r.IsDeleted);

            return await GetAllUsersData(queryViewModel, query);

        }


        private async Task<ResultViewModel<List<UserResultDto>>> GetAllUsersData(QueryViewModel<UserResultDto> queryViewModel, IQueryable<AppUser> query)
        {
            queryViewModel.Filter.ToList().ForEach(x =>
            {
                switch (x.Operation)
                {
                    case (FilterOperation.Equal):
                        if (x.FieldName.ToLower() == "email")
                        {
                            query = query.Where(r => r.Email.ToLower().Contains(x.value.Trim().ToLower()));
                        }
                        else if (x.FieldName.ToLower() == "userid")
                        {
                            query = query.Where(r => r.UserId.ToLower().Contains(x.value.Trim().ToLower()));
                        }

                        break;

                    case (FilterOperation.In):

                        if (x.FieldName.ToLower() == "names")
                        {
                            var vals = x.value.Split(',').Select(int.Parse).ToList();
                            query = query.Where(r => vals.Contains(r.Id));
                        }

                        if (x.FieldName.ToLower() == "groups")
                        {
                            var vals = x.value.Split(',').Select(int.Parse).ToList();
                            query = query.Where(r => r.UserRoles.Select(u => u.RoleId).Where(u => vals.Contains(u)).Count() > 0);
                        }

                        break;
                }
            });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("userid"):
                        query = query.OrderBy(x => x.UserId);
                        break;
                    case ("fullname"):
                        query = query.OrderBy(x => x.FullName);
                        break;
                    case ("email"):
                        query = query.OrderBy(x => x.Email);
                        break;

                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("userid"):
                        query = query.OrderByDescending(x => x.UserId);
                        break;
                    case ("fullname"):
                        query = query.OrderByDescending(x => x.FullName);
                        break;
                    case ("email"):
                        query = query.OrderByDescending(x => x.Email);
                        break;

                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            var Total = query.Count();

            var users = queryViewModel.PageSize == 0 ? query.ToList() : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize).ToList();

            ResultViewModel<List<UserResultDto>> PagedDataResult = new()
            {
                Data = users.Adapt<List<UserResultDto>>(),
                PageSize = queryViewModel.PageSize,
                PageNumber = queryViewModel.PageNumber,
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;
        }

        public ResultViewModel<List<UserResultDto>> GetAllAdmin(QueryViewModel<UserResultDto> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<UserResultDto>>();
            var query = _dbcontext.Users.Where(r => r.IsActive == true && r.IsDeleted != true).Include(x => x.UserRoles).ThenInclude(x => x.Role) as IEnumerable<AppUser>;

            queryViewModel.Filter.ToList().ForEach(x =>
            {
                switch (x.Operation)
                {
                    case (FilterOperation.Equal):
                        if (x.FieldName.ToLower() == "name")
                        {
                            query = query.Where(r => r.UserName.Contains(x.value) || r.FullName.Contains(x.value));
                        }
                        else if (x.FieldName.ToLower() == "email")
                        {
                            query = query.Where(r => r.Email.Contains(x.value));
                        }
                        break;

                    case (FilterOperation.In):
                        if (x.FieldName.ToLower() == "roles")
                        {
                            var vals = x.value.Split(',').Select(int.Parse).ToList();
                            query = query.Where(r => r.UserRoles.Select(u => u.RoleId).Where(u => vals.Contains(u)).Count() > 0);
                        }
                        if (x.FieldName.ToLower() == "status")
                        {
                            var Status = (x.value).Split(',').ToList().Select(a => Convert.ToInt32(a) == 1 ? true : false);

                            query = query.Where(r => Status.Any(a => r.IsActive == a));
                        }
                        break;

                }
            });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("username"):
                        query = query.OrderBy(x => x.UserName);
                        break;
                    case ("fullname"):
                        query = query.OrderBy(x => x.FullName);
                        break;
                    case ("email"):
                        query = query.OrderBy(x => x.Email);
                        break;
                    case ("status"):
                        query = query.OrderBy(x => x.IsActive);
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("username"):
                        query = query.OrderByDescending(x => x.UserName);
                        break;
                    case ("fullname"):
                        query = query.OrderByDescending(x => x.FullName);
                        break;
                    case ("email"):
                        query = query.OrderByDescending(x => x.Email);
                        break;
                    case ("status"):
                        query = query.OrderByDescending(x => x.IsActive);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            var Total = query.Count();

            var users = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            List<UserResultDto> userDtos = users.Adapt<List<UserResultDto>>();

            PagedDataResult.Data = userDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;

        }

        public ResultViewModel<List<UserResultDto>> GetAllUsesrWithSameRole(int userId)
        {
            var PagedDataResult = new ResultViewModel<List<UserResultDto>>();

            var user = _dbcontext.Users.Include(x => x.UserRoles).FirstOrDefault(r => r.IsActive == true && r.IsDeleted != true && r.Id == userId);
            var MasterRoleId = user.UserRoles.FirstOrDefault(a => _dbcontext.Roles.Any(s => s.IsMaster == true && a.RoleId == s.Id))?.RoleId;

            if (MasterRoleId != null)
            {
                var role = _dbcontext.Roles.FirstOrDefault(a => a.Id == MasterRoleId);
                var UsesrWithSameRole = _dbcontext.Users.Include(z => z.UserRoles).Where(w => w.UserRoles.Any(q => q.RoleId == MasterRoleId) && w.Id != user.Id && w.IsActive != true && w.IsDeleted == false);

                List<UserResultDto> userDtos = UsesrWithSameRole.Adapt<List<UserResultDto>>();// _mapper.Map<List<UserResultDto>>(UsesrWithSameRole);

                PagedDataResult.Data = userDtos;
            }


            PagedDataResult.IsSuccess = true;
            return PagedDataResult;

        }

        public async Task<ResultViewModel<UserResultDto>> Getone(int Id)
        {
            var query = _baseService.Context.Users.Where(r => r.Id == Id).Include(x => x.ProfileAttachment).Include(x => x.UserRoles).ThenInclude(x => x.Role).OrderByDescending(a => a.Id).FirstOrDefault();

            ResultViewModel<UserResultDto> result = new() { Data = query.Adapt<UserResultDto>(), IsSuccess = true };
            if (result.Data.UserRoles.Count != 0)
                result.Data.UserRoleId = query.UserRoles.FirstOrDefault().RoleId;
            return result;
        }

        public async Task<ResultViewModel<UserResultDto>> Add(UserAddEditDto userDto)
        {
           // _baseService.CheckValidation(userDto, _validator);

           // List<int> rules = new List<int>();
            //rules.Add((int)userDto.UserRoleId);
            //userDto.UserRoles = rules;
            var user = new AppUser(userDto.UserName, userDto.FullName, userDto.Email, new Guid().ToString()
                       , userDto.UserRoles == null ? null : userDto.UserRoles.Select(x => new UserRole() { RoleId = x }).ToList(), userDto.PhoneNumber, userDto.Address,

                       userDto.Gender, null, userDto.ResidenceCountryId,
                         userDto.CityId, userDto.StateName,
                    (userDto.IsExternal ? EnumUserCategory.ExternalUser : EnumUserCategory.InternalUser), userDto.IsActive);



            await _baseService.Context.Users.AddAsync(user);
            await _baseService.Context.SaveChangesAsync();
            SendCreateUserEmail(user);

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<UserResultDto>> Update(UserAddEditDto userDto)
        {
            ResultViewModel<UserResultDto> _ResultViewModel = new ResultViewModel<UserResultDto>();

            _baseService.CheckValidation(userDto, _validator);



            var user = _baseService.Context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault(x => x.Id == userDto.Id);

            List<int> rules = new List<int>();
            rules.Add((int)userDto.UserRoleId);
            userDto.UserRoles = rules;
            if (userDto.IsActive == false && user.IsActive != false)
            {
                var userConnections = _dbcontext.UserConnections.Where(x => x.AppUserId == user.Id).Select(x => x.ConnectionId.ToString());
                _hub.Clients.Clients(userConnections.ToArray<string>()).SendAsync("LogOut");

            }

            //int userType = 0;
            //userType += userDto.IsLicensedUser == true ? (int)EnumUserType.LicensedUser : 0;
            //userType += userDto.IsSystemAdmin == true ? (int)EnumUserType.SystemAdmin : 0;
            //userType += userDto.IsPMO == true ? (int)EnumUserType.PMO : 0;
            //userType += userDto.IsInitiativesManager == true ? (int)EnumUserType.IsInitiativesManager : 0;
            //userType += userDto.IsStrategyManager == true ? (int)EnumUserType.IsStrategyManager : 0;

            //var licensedUser = new LicensedUser();

            //if (userType == 0)
            //    licensedUser = null;

            //else
            //{
            //    var License = baseService.Context.Licenses.FirstOrDefault(x => x.IsActive && !x.IsDeleted);
            //    var startDate = DateTime.UtcNow;
            //    var endDate = startDate.AddDays(License != null ? License.LicenseDurationDays : 0);
            //    licensedUser = new LicensedUser(startDate, endDate);
            //}


            //var oldUserType = user.UserType;

            user.Update(userDto.UserName, userDto.FullName, userDto.Email, userDto.UserId
                         //, userType
                         //, licensedUser
                         , userDto.Password, _passwordHasher
                         , userDto.UserRoles, userDto.PhoneNumber, userDto.Address, userDto.Gender, userDto.Birthdate.FromISOString(), userDto.ResidenceCountryId,
                         userDto.CityId, userDto.StateName
                         , (userDto.IsExternal ? EnumUserCategory.ExternalUser : EnumUserCategory.InternalUser), userDto.IsActive);

            await _baseService.Context.SaveChangesAsync();



            //if (userType != oldUserType)
            //{
            //    if (oldUserType == 0)
            //        this.SendLicenseUserNotification(user.Id, userDto.Email);

            //    if (userType == 0)
            //        this.SendLicenseUserNotification(user.Id, userDto.Email, true);


            //}

            user = _dbcontext.Users.Include(x => x.ProfileAttachment).FirstOrDefault(x => x.Id == userDto.Id);

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };

            _baseService.sendActionNotification(EnumPageCode.User, EnumActionCode.update, user.Id);


            return result;
        }

        public async Task<ResultViewModel<UserResultDto>> Delete(int id)
        {

            var user = _baseService.Context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                user.Delete();
            }

            await _baseService.Context.SaveChangesAsync();

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };

            _baseService.sendActionNotification(EnumPageCode.User, EnumActionCode.delete, user.Id);

            return result;
        }

        public async Task<ResultViewModel<UserResultDto>> EnableUser(int userID)
        {
            return await SetUserActivity(userID, true);
        }

        public async Task<ResultViewModel<UserResultDto>> DisableUser(int userID)
        {
            return await SetUserActivity(userID, false);
        }


        public async Task<ResultViewModel<UserResultDto>> RecoveryUser(int userID)
        {
            var user =await _baseService.Context.Users.FirstOrDefaultAsync(p => p.Id == userID);

            if (user == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }


            user.RecoveryUser();

            await _baseService.Context.SaveChangesAsync();

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
            return result;
        }

        private async Task<ResultViewModel<UserResultDto>> SetUserActivity(int userID, bool isActive)
        {
            var user = _baseService.Context.Users.FirstOrDefault(p => p.Id == userID && !p.IsDeleted);
            if (user == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }


            if (isActive)
                user.Activate();
            else
                user.Deactivate();

            _baseService.Context.SaveChanges();

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<byte[]> PrintUserReport(QueryViewModel<UserResultDto> queryViewModel, int renderType)
        {

            var reportData = await GetAll(queryViewModel);

            //var report = await _reportService.DownloadReport(reportData.Data.ToList(), "UsersReport", (RenderType)renderType);
            return null;
        }

        public async Task<ResultViewModel<bool>> ImportUserFromExcel(IFormFile formFile)
        {

            string exttension = System.IO.Path.GetExtension(formFile.FileName);
            bool IsSuccess = false;

            var result = new ResultViewModel<bool>();

            if (exttension == ".xlsx")
            {
                if (formFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(_env.ContentRootPath + "/wwwroot/Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(formFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    DataTable dt = new DataTable();
                    try
                    {
                        //Read the connection string for the Excel file.
                        string conString = this._Configuration.GetConnectionString("ExcelConString");

                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _baseService.ExceptionMessages.ReturnExceptionMessages(new ErrorMessageDto() { ErrorMessage = "fileNotSupport", PropertyName = "" });

                    }
                    IsSuccess = await this.AddListOfUsers(dt);

                }
                result.IsSuccess = IsSuccess;
                result.Data = IsSuccess;
                return result;
            }
            else
            {
                _baseService.ExceptionMessages.ReturnExceptionMessages(new ErrorMessageDto() { ErrorMessage = "fileNotSupport", PropertyName = "" });
                result.IsSuccess = false;
                result.Data = false;
                return result;
            }


        }

        private async Task<bool> AddListOfUsers(DataTable dt)
        {
            if (dt.Rows.Count != 0)
            {
                List<AppUser> users = new();
                //int TotalAddLicenses = 0;
                //var License = baseService.Context.Licenses.FirstOrDefault(x => x.IsActive && !x.IsDeleted);
                //int AvaiableLicense = baseService.Context.Users.Include(x => x.licensedUser).Where(s => (s.licensedUser != null && s.licensedUser.IsActive && !s.licensedUser.IsDeleted) && s.IsActive && !s.IsDeleted).Select(x => x.Id).Count();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string? empCode = "";
                    string? FullName = "";
                    string? Email = "";
                    //string? Title = "";
                    string? GroupName = "";
                    //string? Licensed = "";
                    //string? SystemAdmin = "";
                    //string? IsStrategy = "";
                    //string? IsInitiatives = "";
                    //string? PMO = "";
                    string? UserName = "";
                    try
                    {


                        FullName = dt.Rows[i]["Name"].ToString();
                        empCode = dt.Rows[i]["UserID"].ToString();
                        Email = dt.Rows[i]["Email"].ToString();
                        //Title = dt.Rows[i]["Role"].ToString();
                        GroupName = dt.Rows[i]["Group"].ToString();
                        //Licensed = dt.Rows[i]["Licensed"].ToString();
                        //SystemAdmin = dt.Rows[i]["System Admin"].ToString();
                        //PMO = dt.Rows[i]["PMO"].ToString();
                        //IsStrategy = dt.Rows[i]["Is Strategy"].ToString();
                        //IsInitiatives = dt.Rows[i]["Is Initiatives"].ToString();
                        UserName = dt.Rows[i]["UserName"].ToString();
                    }
                    catch (Exception ex)
                    {
                        _baseService.ExceptionMessages.ReturnExceptionMessages(new ErrorMessageDto() { ErrorMessage = "fileNotSupport", PropertyName = "" });
                    }
                    if (empCode != "" || FullName != "" || Email != "")
                    {
                        //int userType = 0;
                        //userType += Licensed == "yes" ? (int)EnumUserType.LicensedUser : 0;
                        //userType += SystemAdmin == "yes" ? (int)EnumUserType.SystemAdmin : 0;
                        //userType += PMO == "yes" ? (int)EnumUserType.PMO : 0;
                        //userType += IsStrategy == "yes" ? (int)EnumUserType.IsStrategyManager : 0;
                        //userType += IsInitiatives == "yes" ? (int)EnumUserType.IsInitiativesManager : 0;

                        List<UserRole> UserRoles = new();
                        if (GroupName != "")
                        {
                            string[] GroupNameList = GroupName.Split(",");

                            foreach (string group in GroupNameList)
                            {
                                var _group = _dbcontext.Roles.Where(r => (r.NameAr.ToLower().Contains(group.Trim().ToLower())
                                                            || r.Name.ToLower().Contains(group.Trim().ToLower()))).FirstOrDefault();
                                if (_group != null)
                                {
                                    UserRoles.Add(new UserRole() { RoleId = _group.Id });
                                }
                            }

                        }

                        //var licensedUser = new LicensedUser();

                        //if (userType == 0)
                        //    licensedUser = null;
                        //else
                        //{
                        //    var startDate = DateTime.UtcNow;
                        //    var endDate = startDate.AddDays(License != null ? License.LicenseDurationDays : 0);
                        //    licensedUser = new LicensedUser(startDate, endDate);
                        //    TotalAddLicenses = TotalAddLicenses + 1;
                        //}


                        // to achieve validate on addeditdto 
                        UserAddEditDto addEditDto = new UserAddEditDto()
                        {
                            Email = Email,
                            FullName = FullName,
                            //IsPMO = PMO == "yes",
                            //IsLicensedUser = Licensed == "yes",
                            //IsSystemAdmin = SystemAdmin == "yes",
                            //IsStrategyManager = IsStrategy == "yes",
                            //IsInitiativesManager = IsInitiatives == "yes",

                            UserName = UserName,
                            UserId = empCode
                        };
                        _baseService.CheckValidation(addEditDto, _validator);

                        // bind add object to model
                        //var user = new AppUser(UserName, FullName, Email, empCode
                        //    //, userType
                        //    //, licensedUser
                        //    , UserRoles, null, null);
                        //users.Add(user);
                    }
                    else
                    {
                        ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "fileIsEmpty", PropertyName = "uploadUsers" };
                        _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
                    }
                }


                //Check if Licenses is Bigger than AvaiableLicense 
                //if (TotalAddLicenses > AvaiableLicense)
                //{

                //    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "LicensesNotEnough", PropertyName = "" };
                //    baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
                //}


                _dbcontext.Users.AddRange(users);
                _dbcontext.SaveChanges();

                foreach (var user in users)
                {
                    _baseService.sendActionNotification(EnumPageCode.User, EnumActionCode.add, user.Id);
                }

                return true;
            }
            else
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "fileIsEmpty", PropertyName = "uploadUsers" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
                return false;
            }
        }

        private void SendCreateUserEmail(AppUser user)
        {
            var token = GetSetPasswordToken(new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName
            });


            string setPasswordLink = GetSysSettings("AdminURL") + "/setPassword/" + token;
            string emailSubject = GetSysSettings("NewUserEmailSubject");

            var emailFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/NewUserEmail/NewUserEmailEn.html", FileMode.Open, FileAccess.Read);
            string emailBody = "";
            using (StreamReader reader = new StreamReader(emailFileStream))
            {
                emailBody = reader.ReadToEnd();
            }
            emailBody = emailBody.Replace("{{Link}}", setPasswordLink);
            var sent = _notificationService.CreateEmailNotification(emailSubject, emailBody, null, new List<string>() { user.Email }, EnumPriority.High);

        }

        public string GetSetPasswordToken(UserDTO userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userDto.UserName.ToString()),
                    new Claim("UserId", userDto.Id.ToString()),
                    new Claim("UserName",userDto.UserName),
                    new Claim("TokenType", ((int)EnumTokenType.SetPassword).ToString()),
                }),
                Expires = DateTime.Now.AddMinutes(int.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "SetPasswordTokenTimeout")?.SysValue ?? "1")),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResultViewModel<UserResultDto>> UpdateProfile(UserProfileEditDto userDto)
        {

            _baseService.CheckValidation(userDto, _profileValidator);

            var user = _dbcontext.Users.FirstOrDefault(x => x.Id == userDto.Id);

            user.UpdateProfile(userDto.PhoneNumber, userDto.Extension, userDto.ProfileAttachmentId);

            await _dbcontext.SaveChangesAsync();

            user = _dbcontext.Users.Include(x => x.ProfileAttachment).FirstOrDefault(x => x.Id == userDto.Id);

            ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
            return result;
        }

        private string GetSysSettings(string sysKey)
        {
            return _dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == sysKey)?.SysValue;
        }


        public Claim[] generateOTPClaims(string otp)
        {
            var sid = DateTime.Now.Ticks.ToString();
            var hash = ComputeStringToSha256Hash(string.Format("{0}:{1}", sid, otp));
            return new Claim[]
            {
                new Claim("otp_id", sid),
                new Claim("otp_hash", hash)
            };
        }

        public string ComputeStringToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
