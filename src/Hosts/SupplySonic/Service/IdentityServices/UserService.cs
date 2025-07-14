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
using Microsoft.AspNetCore.SignalR;
using Service.HubConfig;
using Mapster;
using Microsoft.Extensions.Hosting;
using Core.Enums;
using IServiceContractor.ICommonService;
using FluentValidation;
using System.Data;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using AspNetCore.Reporting;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Core.Models.Attachments;
using DTO.IdentityDTO;
using DTO.CommandDTO;
using IServiceContractor.IdentityInterFaces;
using IServiceContractor.INotificationServices;
using System.Collections;

namespace Service.IdentityServices;

public class UserService : IUserService
{
    private DBContext _dbcontext;
    private readonly AppSettings _appSettings;
    private readonly IHostEnvironment _env;
    private IHubContext<NotificationHub> _hub;
    private readonly IBaseService _baseService;
    private readonly IValidator<UserAddEditDto> _validator;
    private readonly IValidator<UserProfileEditDto> _profileValidator;
    private IConfiguration _Configuration;
    private readonly ReportService _reportService;
    private readonly INotificationService _notificationService;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    public UserService(IBaseService baseService, IValidator<UserAddEditDto> validator, IValidator<UserProfileEditDto> profileValidator, IOptions<AppSettings> options,
        IConfiguration configuration, IHostEnvironment env, IHubContext<NotificationHub> hub, ReportService reportService, INotificationService notificationService, IPasswordHasher<AppUser> passwordHasher)
    {
        _baseService = baseService;

        _validator = validator;
        _profileValidator = profileValidator;
        _dbcontext = _dbcontext;
        _appSettings = options.Value;
        _env = env;
        _hub = hub;
        _Configuration = configuration;
        _reportService = reportService;
        _passwordHasher = passwordHasher;
        _notificationService = notificationService;

        TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
        .Map(dest => dest.FilePath, src => _baseService.CurrentUrlPath + src.Path);

        TypeAdapterConfig<AppUser, UserResultDto>.NewConfig()
        .Map(dest => dest.IsExternal, src => src.UserCategory == EnumUserCategory.ExternalUser)
        .Map(dest => dest.UserRoleNames, src => src.UserRoles != null ? string.Join(", ", src.UserRoles.Select(x => x.Role != null ? baseService.Language == "ar" ? x.Role.NameAr : x.Role.Name : "").ToArray()) : "");

    }

    public async Task<ResultViewModel<List<UserResultDto>>> GetAll(QueryViewModel<UserResultDto> queryViewModel)
    {
        IQueryable<AppUser> query = _dbcontext.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).AsNoTracking()
            .Where(r => r.IsDeleted != true);

        return await GetAllUsersData(queryViewModel, query);
    }

    public async Task<ResultViewModel<List<UserResultDto>>> GetAllActiveUsers(QueryViewModel<UserResultDto> queryViewModel)
    {
        IQueryable<AppUser> query = _dbcontext.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).AsNoTracking()
            .Where(r => r.IsActive && r.IsDeleted != true);

        return await GetAllUsersData(queryViewModel, query);

    }

    public async Task<ResultViewModel<UserResultDto>> Getone(int Id)
    {
        var query = await _dbcontext.Users.AsNoTracking().Include(x => x.ProfileAttachment).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(r => r.Id == Id);
        ResultViewModel<UserResultDto> result = new() { Data = query.Adapt<UserResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<UserResultDto>> Add(UserAddEditDto userDto)
    {
        _baseService.CheckValidation(userDto, _validator);

        var user = new AppUser(userDto.UserName, userDto.FullName, userDto.Email, userDto.UserId
            , userDto.UserRoles == null ? null : userDto.UserRoles.Select(x => new UserRole() { RoleId = x }).ToList()
            , userDto.IsExternal ? EnumUserCategory.ExternalUser : EnumUserCategory.InternalUser, userDto.IsActive);

        await _dbcontext.Users.AddAsync(user);
        await _dbcontext.SaveChangesAsync();

        _baseService.sendActionNotification(EnumPageCode.User, EnumActionCode.add, user.Id);
        SendCreateUserEmail(user);

        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<UserResultDto>> Update(UserAddEditDto userDto)
    {
        ResultViewModel<UserResultDto> _ResultViewModel = new ResultViewModel<UserResultDto>();

        _baseService.CheckValidation(userDto, _validator);

        var user = await _dbcontext.Users.Include(s => s.ProfileAttachment).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Id == userDto.Id);

        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "" };
            _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
        }

        //if (!StructuralComparisons.StructuralEqualityComparer.Equals(user.RowVersion, userDto.RowVersion))
        //{
        //    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
        //    _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        //}

        if (userDto.IsActive == false && user.IsActive != false)
        {
            var userConnections = _dbcontext.UserConnections.Where(x => x.AppUserId == user.Id).Select(x => x.ConnectionId.ToString());
            _hub.Clients.Clients(userConnections.ToArray()).SendAsync("LogOut");
        }

        user.Update(userDto.UserName, userDto.FullName, userDto.Email, userDto.UserId, userDto.Password, _passwordHasher, userDto.UserRoles,
                    userDto.IsExternal ? EnumUserCategory.ExternalUser : EnumUserCategory.InternalUser, userDto.IsActive);

        await _dbcontext.SaveChangesAsync();

        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };

        return result;
    }

    public async Task<ResultViewModel<UserResultDto>> Delete(int id)
    {

        var user = _dbcontext.Users.FirstOrDefault(x => x.Id == id);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "" };
            _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
        }
        user.Delete();
        await _dbcontext.SaveChangesAsync();

        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
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

    public async Task<byte[]> PrintUserReport(QueryViewModel<UserResultDto> queryViewModel, int renderType)
    {

        var reportData = await GetAll(queryViewModel);

        var report = await _reportService.DownloadReport(reportData.Data.ToList(), "UsersReport", (RenderType)renderType);
        return report.MainStream;
    }

    public async Task<ResultViewModel<bool>> ImportUserFromExcel(IFormFile formFile)
    {

        string exttension = Path.GetExtension(formFile.FileName);
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
                    string conString = _Configuration.GetConnectionString("ExcelConString");

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
                IsSuccess = await AddListOfUsers(dt);

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

    public async Task<ResultViewModel<UserResultDto>> UpdateProfile(UserProfileEditDto userDto)
    {
        _baseService.CheckValidation(userDto, _profileValidator);

        var user = await _dbcontext.Users.Include(x => x.ProfileAttachment).FirstOrDefaultAsync(x => x.Id == userDto.Id);

        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "" };
            _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
        }

        user.UpdateProfile(userDto.PhoneNumber, userDto.Extension, userDto.ProfileAttachmentId);

        await _dbcontext.SaveChangesAsync();

        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
        return result;
    }


    #region Helpers Funactions

    string GetSysSettings(string sysKey)
    {
        return _dbcontext.SysSettings.AsNoTracking().FirstOrDefault(x => x.SysKey == sysKey)?.SysValue;
    }
    string GetSetPasswordToken(UserDTO userDto)
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
    void SendCreateUserEmail(AppUser user)
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

        //var sent = _emailSender.SendEmailAsync(user.Email, emailSubject, emailBody).Result;

    }

    async Task<bool> AddListOfUsers(DataTable dt)
    {
        if (dt.Rows.Count != 0)
        {
            List<AppUser> users = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string? empCode = "", FullName = "", Email = "", GroupName = "", UserName = "";

                try
                {
                    FullName = dt.Rows[i]["Name"].ToString();
                    empCode = dt.Rows[i]["UserID"].ToString();
                    Email = dt.Rows[i]["Email"].ToString();
                    GroupName = dt.Rows[i]["Group"].ToString();
                    UserName = dt.Rows[i]["UserName"].ToString();
                }
                catch (Exception ex)
                {
                    _baseService.ExceptionMessages.ReturnExceptionMessages(new ErrorMessageDto() { ErrorMessage = "fileNotSupport", PropertyName = "" });
                }

                if (empCode != "" || FullName != "" || Email != "")
                {
                    List<UserRole> UserRoles = new();
                    if (GroupName != "")
                    {
                        string[] GroupNameList = GroupName.Split(",");

                        foreach (string group in GroupNameList)
                        {
                            var _group = _dbcontext.Roles.AsNoTracking().Where(r => r.NameAr.ToLower().Contains(group.Trim().ToLower())
                                                        || r.Name.ToLower().Contains(group.Trim().ToLower())).FirstOrDefault();
                            if (_group != null)
                            {
                                UserRoles.Add(new UserRole() { RoleId = _group.Id });
                            }
                        }

                    }

                    // to achieve validate on addeditdto 
                    UserAddEditDto addEditDto = new UserAddEditDto()
                    {
                        Email = Email,
                        FullName = FullName,
                        UserName = UserName,
                        UserId = empCode
                    };
                    _baseService.CheckValidation(addEditDto, _validator);

                    // bind add object to model
                    var user = new AppUser(UserName, FullName, Email, empCode, UserRoles);
                    users.Add(user);
                }
                else
                {
                    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "fileIsEmpty", PropertyName = "uploadUsers" };
                    _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
                }
            }

            _dbcontext.Users.AddRange(users);
            _dbcontext.SaveChanges();

            return true;
        }
        else
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "fileIsEmpty", PropertyName = "uploadUsers" };
            _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
            return false;
        }
    }

    async Task<ResultViewModel<List<UserResultDto>>> GetAllUsersData(QueryViewModel<UserResultDto> queryViewModel, IQueryable<AppUser> query)
    {
        queryViewModel.Filter.ToList().ForEach(x =>
        {
            switch (x.Operation)
            {
                case FilterOperation.Equal:
                    if (x.FieldName.ToLower() == "email")
                    {
                        query = query.Where(r => r.Email.ToLower().Contains(x.value.Trim().ToLower()));
                    }
                    else if (x.FieldName.ToLower() == "userid")
                    {
                        query = query.Where(r => r.UserId.ToLower().Contains(x.value.Trim().ToLower()));
                    }

                    break;

                case FilterOperation.In:

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
                case "userid":
                    query = query.OrderBy(x => x.UserId);
                    break;
                case "fullname":
                    query = query.OrderBy(x => x.FullName);
                    break;
                case "email":
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
                case "userid":
                    query = query.OrderByDescending(x => x.UserId);
                    break;
                case "fullname":
                    query = query.OrderByDescending(x => x.FullName);
                    break;
                case "email":
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

    async Task<ResultViewModel<UserResultDto>> SetUserActivity(int userID, bool isActive)
    {
        var user = _dbcontext.Users.FirstOrDefault(p => p.Id == userID && !p.IsDeleted);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
            _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
        }


        if (isActive)
            user.Activate();
        else
            user.Deactivate();

        _dbcontext.SaveChanges();

        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };
        return result;
    }

    #endregion
}
