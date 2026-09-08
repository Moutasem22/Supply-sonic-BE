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

namespace Service;

public class SupplierAppUserService : ISupplierAppUserService
{
    private DBContext _dbcontext;
    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _env;
    private IHubContext<NotificationHub> _hub;
    private readonly IBaseService _baseService;
    private readonly IValidator<SupplierAppUserAddEditDto> _validator;
    private readonly IValidator<UserProfileEditDto> _profileValidator;
    private IConfiguration _Configuration;

    private readonly INotificationService _notificationService;
    private readonly IPasswordHasher<SupplierAppUser> _passwordHasher;
    public SupplierAppUserService(IBaseService baseService, IValidator<SupplierAppUserAddEditDto> validator, IValidator<UserProfileEditDto> profileValidator, IOptions<AppSettings> options,
        IConfiguration configuration, IHostEnvironment env, IHubContext<NotificationHub> hub, INotificationService notificationService, IPasswordHasher<SupplierAppUser> passwordHasher)
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

        TypeAdapterConfig<SupplierAppUser, UserResultDto>.NewConfig()
        .Map(dest => dest.IsExternal, src => src.UserCategory == EnumUserCategory.ExternalUser)
        .Map(dest => dest.CityName, src => src.City != null ? _baseService.Language == "ar" ? src.City.NameAr : src.City.NameEn : null)
        .Map(dest => dest.CountryName, src => src.ResidenceCountry != null ? _baseService.Language == "ar" ? src.ResidenceCountry.NameAr : src.ResidenceCountry.NameEn : null);


    }



    public async Task<ResultViewModel<List<UserResultDto>>> GetAll(QueryViewModel<UserResultDto> queryViewModel)
    {
        IQueryable<SupplierAppUser> query = _baseService.Context.SupplierAppUsers.Include(s => s.ProfileAttachment)
            .Where(r => r.IsDeleted != true);

        return await GetAllUsersData(queryViewModel, query);
    }

    public async Task<ResultViewModel<List<UserResultDto>>> GetAllActiveUsers(QueryViewModel<UserResultDto> queryViewModel)
    {
        IQueryable<SupplierAppUser> query = _baseService.Context.SupplierAppUsers
            .Where(r => r.IsActive && r.IsDeleted != true);

        return await GetAllUsersData(queryViewModel, query);

    }

    private async Task<ResultViewModel<List<UserResultDto>>> GetAllUsersData(QueryViewModel<UserResultDto> queryViewModel, IQueryable<SupplierAppUser> query)
    {
        queryViewModel.Filter.ToList().ForEach(x =>
        {
            switch (x.Operation)
            {
                case (FilterOperation.Equal):
                    if (x.FieldName.ToLower() == "name")
                    {
                        query = query.Where(r => r.FullName.ToLower().Contains(x.value.Trim().ToLower()));
                    }
                    else if (x.FieldName.ToLower() == "email")
                    {
                        query = query.Where(r => r.Email.ToLower().Contains(x.value.Trim().ToLower()));
                    }
                    else if (x.FieldName.ToLower() == "userid")
                    {
                        query = query.Where(r => r.UserId.ToLower().Contains(x.value.Trim().ToLower()));
                    }
                    else if (x.FieldName.ToLower() == "phonenumber")
                    {
                        query = query.Where(r => r.PhoneNumber.Contains(x.value));
                    }

                    break;

                case (FilterOperation.In):

                    if (x.FieldName.ToLower() == "names")
                    {
                        var vals = x.value.Split(',').Select(int.Parse).ToList();
                        query = query.Where(r => vals.Contains(r.Id));
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
        var query = _dbcontext.Users.Where(r => r.IsActive == true && r.IsDeleted != true).Include(x => x.UserRoles).ThenInclude(x => x.Role) as IEnumerable<SupplierAppUser>;

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
                    else if (x.FieldName.ToLower() == "phonenumber")
                    {
                        query = query.Where(r => r.PhoneNumber.Contains(x.value));
                    }
                    break;

                case (FilterOperation.In):

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
        var query = _baseService.Context.SupplierAppUsers.AsNoTracking().Include(s => s.ProfileAttachment).FirstOrDefault(r => r.Id == _baseService.CurrentUserId);
        ResultViewModel<UserResultDto> result = new() { Data = query.Adapt<UserResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<UserResultDto>> GetoneAdmin(int Id)
    {
        var query = await _baseService.Context.SupplierAppUsers.AsNoTracking()
                                              .Include(s => s.ProfileAttachment)
                                              .Include(s => s.City).Include(s => s.ResidenceCountry)
                                              .FirstOrDefaultAsync(r => r.Id == Id);
        ResultViewModel<UserResultDto> result = new() { Data = query.Adapt<UserResultDto>(), IsSuccess = true };

        //result.Data.beneficiaries= _baseService.Context.SupplierUserBeneficiaries.AsNoTracking().Where(s=>s.UserId == Id).ToList().Adapt<List<SupplierUserBeneficiaryResultDto>>();
        return result;
    }



    public async Task<ResultViewModel<UserInfo>> Add(SupplierAppUserAddEditDto userDto)
    {
        _baseService.CheckValidation(userDto, _validator);
        var otp = GeneratOTP();
        var otptime = GetSysSettings("OTPTimeOut") ?? "2";
        var user = new SupplierAppUser(userDto.UserName, userDto.FullName, userDto.Email, otp,
            DateTime.UtcNow.AddMinutes(int.Parse(otptime))
            ,
            "00", (bool)userDto.IsSupplier, userDto.ProfileAttachmentId);
        //var passwordHash = _passwordHasher.HashPassword(user, userDto.Password);
        user.SetPassword(userDto.Password, _passwordHasher);
        _baseService.Context.SupplierAppUsers.Add(user);
        _baseService.Context.SaveChanges();
        var result = new ResultViewModel<UserInfo>();

        result.Data = new UserInfo
        {
            AppUsers = user.Adapt<UserDTO>(),
            claimsIdentity = null,
            //Token = user.Token,
            OtpCode = otp,
            OtpTimeOut = int.Parse(otptime),
            NeedOTP = true
        };

        SendCreateUserEmail(user, otp);

        return result;
    }

    private string GeneratOTP()
    {
        var digit = 4;
        var otp = new Random().Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit) - 1).ToString("####");
        //if (otp[0] == '0')
        //    return GeneratOTP(user);
        //user.Token = GetGuestToken(new UserDTO()
        //{
        //    Id = user.Id,
        //    UserName = user.UserName
        //}, otp);
        return otp;
    }

    public string GetGuestToken(UserDTO userDto, string otpCode)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.UserName.ToString()),
                new Claim("UserId", userDto.Id.ToString()),
                new Claim("UserName", userDto.UserName),
                new Claim("TokenType", ((int)EnumTokenType.LoginOtp).ToString()),
                new Claim("LongOTPExpire", DateTime.Now.AddMinutes(int.Parse(GetSysSettings("LongOTPExpire") ?? "5")).ToString("yyyy-MM-dd HH:mm")),
            };

        claims.AddRange(generateOTPClaims(otpCode));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(int.Parse(GetSysSettings("OTPTimeOut") ?? "10")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<ResultViewModel<UserResultDto>> Update(SupplierAppUserAddEditDto userDto)
    {
        ResultViewModel<UserResultDto> _ResultViewModel = new ResultViewModel<UserResultDto>();

        _baseService.CheckValidation(userDto, _validator);

        var user = _baseService.Context.SupplierAppUsers.FirstOrDefault(x => x.Id == userDto.Id);

        user.Update(userDto.UserName, userDto.FullName, userDto.Email, "1"
                     //, userType
                     //, licensedUser
                     , userDto.Password, _passwordHasher
                   );

        await _baseService.Context.SaveChangesAsync();


        user = _dbcontext.SupplierAppUsers.Include(x => x.ProfileAttachment).FirstOrDefault(x => x.Id == userDto.Id);

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

    private async Task<ResultViewModel<UserResultDto>> SetUserActivity(int userID, bool isActive)
    {
        var user = _baseService.Context.SupplierAppUsers.FirstOrDefault(p => p.Id == userID && !p.IsDeleted);
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

        //  var report = await _reportService.DownloadReport(reportData.Data.ToList(), "UsersReport", (RenderType)renderType);
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



    private void SendCreateUserEmail(SupplierAppUser user, string otp)
    {
        var token = GetSetPasswordToken(new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName
        });


        string emailSubject = GetSysSettings("NewUserEmailSubject");

        var emailFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/NewUserEmail/NewUserEmailAr.html", FileMode.Open, FileAccess.Read);
        string emailBody = "";
        using (StreamReader reader = new StreamReader(emailFileStream))
        {
            emailBody = reader.ReadToEnd();
        }
        emailBody = emailBody.Replace("{{OTP}}", otp);
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

    public async Task<ResultViewModel<UserResultDto>> UpdateProfile(SupplierAppUserAddEditDto userDto)
    {

        //_baseService.CheckValidation(userDto, _validator);

        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => x.Id == _baseService.CurrentUserId);

        user.UpdateProfile(userDto.PhoneNumber, userDto.ProfileAttachmentId, userDto.FullName,
                           userDto.ResidenceCountryId, userDto.CityId, userDto.Address);

        await _dbcontext.SaveChangesAsync();

        user = _dbcontext.SupplierAppUsers.Include(x => x.ProfileAttachment).FirstOrDefault(x => x.Id == userDto.Id);

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

    public async Task<ResultViewModel<UserResultDto>> AddUserAddress(SupplierAppUserAddEditDto userDto)
    {
        ResultViewModel<UserResultDto> _ResultViewModel = new ResultViewModel<UserResultDto>();
        //_baseService.CheckValidation(userDto, _validator);
        var user = _baseService.Context.SupplierAppUsers.FirstOrDefault(x => x.Id == _baseService.CurrentUserId);
        user.UpdateAddress(userDto.ResidenceCountryId, userDto.CityId, userDto.PhoneNumber, userDto.Address);
        await _baseService.Context.SaveChangesAsync();
        user = _dbcontext.SupplierAppUsers.Include(x => x.ProfileAttachment).FirstOrDefault(x => x.Id == userDto.Id);
        ResultViewModel<UserResultDto> result = new() { Data = user.Adapt<UserResultDto>(), IsSuccess = true };

        return result;
    }

    public async Task<ResultViewModel<bool>> UpdateStatus(SupplierAppUserAddEditDto userDto)
    {
        ResultViewModel<bool> _ResultViewModel = new ResultViewModel<bool>();

        var user = _baseService.Context.SupplierAppUsers.FirstOrDefault(x => x.Id == userDto.Id);

        user.UpdateStatus(userDto.Status, userDto.RejectedReason);

        await _baseService.Context.SaveChangesAsync();


        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };

        return result;
    }
}
