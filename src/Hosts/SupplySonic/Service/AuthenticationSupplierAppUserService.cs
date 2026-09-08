using DTO;
using Core.Models.Identity;
using DB;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IServiceContractor;
using Mapster;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Localization;
using Microsoft.Extensions.Hosting;
using Core.Enums;
using IServiceContractor.ICommonService;
using FluentValidation;
using System.Data;
using System.Collections.Generic;

namespace Service;

public class AuthenticationSupplierAppUserService : IAuthenticationSupplierAppUserService
{
    private readonly SignInManager<SupplierAppUser> _signInManager;
    private DBContext _dbcontext;
    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordHasher<SupplierAppUser> _passwordHasher;
    private readonly ITokenFactory _tokenFactory;
    private readonly IJwtTokenValidator _jwtTokenValidator;
    private readonly IHostEnvironment _env;
    private readonly IBaseService _baseService;
    private readonly IValidator<PasswordDto> _passwordValidator;
    private readonly IValidator<SetPasswordDto> _setPasswordValidator;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
    private readonly INotificationService _notificationService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public AuthenticationSupplierAppUserService(IBaseService baseService, IValidator<PasswordDto> passwordValidator, IValidator<SetPasswordDto> setPasswordValidator, IValidator<ResetPasswordDto> resetPasswordValidator, IOptions<AppSettings> options,
        ITokenFactory tokenFactory, IJwtTokenValidator jwtTokenValidator, IHostEnvironment env, INotificationService notificationService, SignInManager<SupplierAppUser> signInManager, IPasswordHasher<SupplierAppUser> passwordHasher)
    {
        this._baseService = baseService;
        this._passwordValidator = passwordValidator;
        this._resetPasswordValidator = resetPasswordValidator;
        this._setPasswordValidator = setPasswordValidator;
        //_signInManager = baseService.SignInManager;
        _dbcontext = _baseService.Context;
        _appSettings = options.Value;
        _localizer = baseService.Localizer;
        _httpContextAccessor = baseService.HttpContextAccessor;
        _passwordHasher = passwordHasher;
        _tokenFactory = tokenFactory;
        _jwtTokenValidator = jwtTokenValidator;
        _env = env;
        _signInManager = signInManager;
        this._notificationService = notificationService;
    }


    #region old Authenticate oldAuthenticate
    public ResultViewModel<UserInfo> Authenticate(string username, string password, string RemoteIpAddress)
    {
        var result = new ResultViewModel<UserInfo>();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUsernameorPasswordMsg", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        bool? isEmail = username.IndexOf('@') == -1 ? false : true;

        var user = _dbcontext.SupplierAppUsers.Include(x => x.RefreshTokens).Include(x => x.ProfileAttachment).FirstOrDefault(x => ((isEmail == true && x.Email == username) || x.UserName == username || x.Email == username) && x.IsDeleted != true);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUsernameorPasswordMsg", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.IsActive != true)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "msg_AccountIsDisabled", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }



        try
        {
            var respons = _signInManager.CheckPasswordSignInAsync(user, password, true).Result;
            if (!respons.Succeeded)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUsernameorPasswordMsg", PropertyName = "" };
                _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
            }
        }
        catch (Exception ex)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUsernameorPasswordMsg", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.EmailConfirmed != true)
        {
            return ResendOTP(user?.Id);
        }


        user.UpdateToken(GetVerifiedToken(new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName
        }));



        var refreshToken = _tokenFactory.GenerateToken();
        user.RemoveAllRefreshToken();
        user.AddRereshToken(refreshToken, RemoteIpAddress, double.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "RefreshTokenTimeout")?.SysValue ?? "2"));
        _dbcontext.SaveChanges();

        var userdto = user.Adapt<UserDTO>();

        result.IsSuccess = true;
        result.Data = new UserInfo
        {
            AppUsers = userdto,
            claimsIdentity = null,
            Token = user.Token,
            RefreshToken = refreshToken,
            NeedOTP = !user.EmailConfirmed,
            IsSupplier = user.IsSupplier,

        };


        return result;
    }
    #endregion

    public string GetExternalSystemToken(UserDTO userDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.UserName.ToString()),
                new Claim("UserId", userDto.Id.ToString()),
                new Claim("UserName",userDto.UserName),
                new Claim("externalSystem","true"),
            };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(int.Parse(GetSysSettings("ExternalAccessTokenTimeout") ?? "1")),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public ExtrnalToken AuthenticateExternalSystem(string username, string password)
    {

        var result = new ExtrnalToken();
        if (string.IsNullOrWhiteSpace(password))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "EmptyPassword", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => (x.UserName == username || x.Email == username) && x.IsDeleted != true);

        // return null if user not found
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUserPassword", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.IsActive != true)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "UserDisabled", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var respons = _signInManager.CheckPasswordSignInAsync(user, password, true).Result;
        if (!respons.Succeeded)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUserPassword", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }


        var Token = GetExternalSystemToken(new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName
        });

        var userdto = new UserDTO();
        userdto.UserName = user.UserName;
        userdto.FullName = user.FullName;//user.FirstName + " " + user.LastName;

        userdto.Id = user.Id;



        result = new ExtrnalToken
        {
            Token = Token,
            TokenDuration = int.Parse(GetSysSettings("ExternalAccessTokenTimeout") ?? "1")
        };
        return result;

    }
    public ResultViewModel<UserInfo> Verify(VerifyDto verifyDto)
    {
        //var handler = new JwtSecurityTokenHandler();
        //var jsonToken = handler.ReadToken(verifyDto.Token);
        //var tokenS = handler.ReadToken(verifyDto.Token) as JwtSecurityToken;
        //var claims = tokenS?.Claims;

        var result = new ResultViewModel<UserInfo>();
        //var otp_id = claims.FirstOrDefault(x => x.Type == "otp_id").Value;
        //var otp_hash = claims.FirstOrDefault(x => x.Type == "otp_hash").Value;

        //if (!VerifyOtp(otp_id, verifyDto.VerificationCode, otp_hash))
        //{
        //    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalideCode", PropertyName = "" };
        //    _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        //}

        //var userId = claims.SingleOrDefault(x => x.Type == "UserId")?.Value;
        //if (!int.TryParse(userId, out int userIdResult))
        //{
        //    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalideUser", PropertyName = "" };
        //    _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        //}

        var user = _dbcontext.SupplierAppUsers.Include(x => x.RefreshTokens).FirstOrDefault(x => x.Id == verifyDto.UserId && x.IsDeleted != true);

        if (verifyDto.VerificationCode != user.OtpCode || user.OtpDate < DateTime.UtcNow)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalideCode", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        user.UpdateToken(GetVerifiedToken(new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName,
        }));
        var refreshToken = _tokenFactory.GenerateToken();
        user.RemoveAllRefreshToken();
        user.AddRereshToken(refreshToken, verifyDto.RemoteIpAddress, double.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "RefreshTokenTimeout")?.SysValue ?? "2"));
        _dbcontext.SaveChanges();

        // remove password before returning
        var userdto = new UserDTO();
        userdto.UserName = user.UserName;
        userdto.FullName = user.FullName;
        userdto.Id = user.Id;

        result.IsSuccess = true;
        result.Data = new UserInfo
        {
            AppUsers = userdto,
            claimsIdentity = null,
            Token = user.Token,
            RefreshToken = refreshToken
        };
        return result;
    }
    public ResultViewModel<bool> Logout(IEnumerable<Claim> claims)
    {
        var result = new ResultViewModel<bool>();
        var UserId = int.Parse(claims.First(c => c.Type == "UserId").Value);
        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => x.Id == UserId);
        if (user != null)
        {
            user.LogOut();
            _dbcontext.SaveChanges();
        }
        result.IsSuccess = true;
        result.Data = true;
        return result;
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
            Expires = DateTime.Now.AddMinutes(int.Parse(GetSysSettings("OTPTimeOut") ?? "2")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GetSysSettings(string sysKey)
    {
        return _dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == sysKey)?.SysValue;
    }

    public string GetVerifiedToken(UserDTO userDto)
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
                    new Claim("TokenType", ((int)EnumTokenType.Verified).ToString()),
            }),
            Expires = DateTime.Now.AddMinutes(int.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "AccessTokenTimeout")?.SysValue ?? "1")),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool VerifyOtp(string otpId, string otp, string otpHash)
    {
        if (string.IsNullOrEmpty(otpId) || string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(otpHash))
            return false;

        return ComputeStringToSha256Hash(string.Format("{0}:{1}", otpId, otp)) == otpHash;
    }
    public ResultViewModel<UserInfo> GetAccessTokenUsingRefreshToken(string OldAccessToken, string RefreshToken)
    {
        var result = new ResultViewModel<UserInfo>();
        try
        {
            var cp = _jwtTokenValidator.GetPrincipalFromToken(OldAccessToken, _appSettings.Secret);

            if (cp != null)
            {
                var UserId = int.Parse(cp.Claims.First(c => c.Type == "UserId").Value);
                var user = _dbcontext.SupplierAppUsers.Include(x => x.RefreshTokens).FirstOrDefault(x => x.Id == UserId);
                if (user.Token != OldAccessToken)
                {
                    ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "LoginFromOtherDevice", PropertyName = "" };
                    _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
                }
                if (user.HasValidRefreshToken(System.Net.WebUtility.HtmlDecode(RefreshToken)))
                {
                    var jwtToken = GetVerifiedToken(new UserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName
                    });
                    var refreshToken = _tokenFactory.GenerateToken();
                    //user.RemoveRefreshToken(System.Net.WebUtility.HtmlDecode(RefreshToken)); // delete the token we've exchanged
                    user.RemoveAllRefreshToken();
                    user.AddRereshToken(refreshToken, "", double.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "RefreshTokenTimeout")?.SysValue ?? "5")); // add the new one
                    user.UpdateToken(jwtToken);
                    _dbcontext.SaveChanges();

                    var userdto = new UserDTO();
                    userdto.UserName = user.UserName;
                    userdto.FullName = user.FullName;//user.FirstName + " " + user.LastName;
                    userdto.Id = user.Id;
                    result.IsSuccess = true;

                    result.Data = new UserInfo
                    {
                        AppUsers = userdto,
                        claimsIdentity = null,
                        Token = jwtToken,
                        RefreshToken = refreshToken
                    };
                    return result;

                }
            }

        }
        catch (Exception ex)
        {

            //// comment by sayed it will be handle in futrue (18-5-2022)
            //  ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidToken", PropertyName = "" };
            //baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        return result;

    }

    public UserWithPagesDTO UserInfoAndPages(int userId, string currentPageCode = "")
    {
        var UserWithPagesDTO = new UserWithPagesDTO();
        var userInfo = getItem(userId);
        UserWithPagesDTO.CurrentUser = userInfo;
        var qp = _dbcontext.Pages.FromSqlRaw($"exec sp_GetAuthorizePages @userId= {userId}").ToList();
        var AuthorizedPages = new ResultViewModel<List<PageDTO>>
        {
            IsSuccess = true,
            Data = qp.ToList().Adapt<List<PageDTO>>(),//_mapper.Map<List<PageDTO>>(qp.ToList()),
            Total = qp.Count()
        };
        UserWithPagesDTO.AuthorizedPages = AuthorizedPages;

        var qpc = _dbcontext.PageCategories.Where(x => x.IsActive == true && AuthorizedPages.Data.Select(y => y.PageCategoryId).Contains(x.Id)).ToList();
        var pageCategories = new ResultViewModel<List<PageCategoryDTO>>
        {
            IsSuccess = true,
            Data = qpc.ToList().Adapt<List<PageCategoryDTO>>(),//_mapper.Map<List<PageCategoryDTO>>(qpc.ToList()),
            Total = qpc.Count()
        };
        UserWithPagesDTO.PageCategories = pageCategories;

        if (!string.IsNullOrWhiteSpace(currentPageCode))
        {
            var qa = _dbcontext.Actions.FromSqlRaw($"sp_GetAuthorizeActions @userId={userId},@pageCode='{currentPageCode}'").ToList();

            var AuthorizedActions = new ResultViewModel<List<ActionDTO>>
            {
                IsSuccess = true,
                Data = qa.ToList().Adapt<List<ActionDTO>>(), //_mapper.Map<List<ActionDTO>>( qa.ToList()),
                Total = qa.Count()
            };

            UserWithPagesDTO.AuthorizedCurrentPageAction = AuthorizedActions;

        }
        return UserWithPagesDTO;
    }
    public UserDTO getItem(int Id)
    {

        var res = _dbcontext.SupplierAppUsers
            .FirstOrDefault(x => x.Id == Id);


        var userdto = res.Adapt<UserDTO>();// _mapper.Map<UserDTO>(res);             
        return userdto;

    }

    public ResultViewModel<UserInfo> ResendOTP(int? userId)
    {
        var result = new ResultViewModel<UserInfo>();
       // var cp = _jwtTokenValidator.GetPrincipalFromToken(Token, _appSettings.Secret);
        //if (cp == null)
        //{
         //   result.Messages.Add(new MessageModel() { InputName = "OTP", Message = _localizer["TokenTimeOut"] });
          //  return result;
       // }
        //var LongOTPExpire = DateTime.Parse(cp.Claims.First(c => c.Type == "LongOTPExpire").Value);
        //if (LongOTPExpire < DateTime.Now)
        //{
        //    result.Messages.Add(new MessageModel() { InputName = "OTP", Message = _localizer["TokenTimeOut"] });

        //    return result;
        //}
        //var userId = int.Parse(cp.Claims.First(c => c.Type == "UserId").Value);
        var user = _baseService.Context.SupplierAppUsers.FirstOrDefault(x => x.Id == userId);
 
        var otp = GeneratOTP();
        var otptime = GetSysSettings("OTPTimeOut") ?? "2";
        user.SendOtp(otp,DateTime.UtcNow.AddMinutes(int.Parse(otptime)));
        _baseService.Context.SaveChanges();
        var userdto = new UserDTO();
        userdto.UserName = user.UserName;
        userdto.FullName = user.FullName;//user.FirstName + " " + user.LastName;
        userdto.Id = user.Id;
        result.IsSuccess = true;


        result.Data = new UserInfo
        {
            AppUsers = userdto,
            claimsIdentity = null,
           // Token = user.Token,
            OtpCode = otp,
            OtpTimeOut = int.Parse(GetSysSettings("OTPTimeOut") ?? "2"),
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
    ///////change Password function/////////////////////////
    public async Task<ResultViewModel<bool>> ChangePassword(PasswordDto dto)
    {
        var result = new ResultViewModel<bool>();
        _baseService.CheckValidation(dto, _passwordValidator);

        //int userId;
        //bool chkParse;
        //chkParse = int.TryParse(_httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value, out userId);

        int userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value);
        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(a => a.Id == userId);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }


        var respons = _signInManager.CheckPasswordSignInAsync(user, dto.OldPassword, true);
        if (respons.Exception != null)
        {
            throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(new ValidationResult("TheOldPasswordIsNotCorrect", new List<string>() { nameof(dto.OldPassword) })));

        }
        var responsResult = respons.Result;
        if (!responsResult.Succeeded)
        {
            result.Data = false;
            return result;
        }
        else
        {
            var newPasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            user.PasswordHash = newPasswordHash;
            _dbcontext.SupplierAppUsers.Update(user);
            await _dbcontext.SaveChangesAsync();
            result.Data = true;
            result.IsSuccess = true;
            return result;
        }

        return result;
    }

    public ResultViewModel<List<UserResultDto>> GetAllUsersForNotification(QueryViewModel<UserResultDto> queryViewModel)
    {
        var PagedDataResult = new ResultViewModel<List<UserResultDto>>();

        var query = _dbcontext.SupplierAppUsers.Where(r => r.IsActive == true && r.IsDeleted != true) as IEnumerable<SupplierAppUser>;

        queryViewModel.Filter.ToList().ForEach(x =>
        {
            switch (x.Operation)
            {
                case (FilterOperation.Equal):
                    if (x.FieldName.ToLower() == "name")
                    {
                        query = query.Where(r => r.FullName != null && r.FullName.Contains(x.value));
                    }
                    else if (x.FieldName.ToLower() == "nationalid")
                    {
                        query = query.Where(r => r.UserName != null && r.UserName.Contains(x.value));
                    }
                    else if (x.FieldName.ToLower() == "phonenumber")
                    {
                        query = query.Where(r => r.PhoneNumber != null && r.PhoneNumber.Contains(x.value));
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
                default:
                    query = query.OrderByDescending(x => x.Id);
                    break;
            }
        }

        var Total = query.Count();

        var users = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

        List<UserResultDto> userDtos = users.Adapt<List<UserResultDto>>();//_mapper.Map<List<UserResultDto>>(users);

        PagedDataResult.Data = userDtos;
        PagedDataResult.PageSize = queryViewModel.PageSize;
        PagedDataResult.PageNumber = queryViewModel.PageNumber;
        PagedDataResult.Total = Total;
        PagedDataResult.IsSuccess = true;
        return PagedDataResult;


    }

    public bool CheckUserIfSuperAdmin(string UserId)
    {

        var query = _dbcontext.SupplierAppUsers.Where(r => r.IsActive == true && r.IsDeleted != true && r.Id == int.Parse(UserId)).FirstOrDefault();
        return query.IsSuperAdmin;

    }

    public string GetForgetPasswordToken(UserDTO userDto)
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
                    new Claim("TokenType", ((int)EnumTokenType.ForgetPassword).ToString()),
            }),
            Expires = DateTime.Now.AddMinutes(int.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "ForgetPasswordTokenTimeout")?.SysValue ?? "1")),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ResultViewModel<bool> ForgetPassword(string email)
    {
        var result = new ResultViewModel<bool>();

        if (string.IsNullOrWhiteSpace(email))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidEmail", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => (x.Email == email) && x.IsDeleted != true);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidEmail", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.IsActive != true)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "msg_AccountIsDisabled", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var token = GetForgetPasswordToken(new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName
        });

        var adminURL = GetSysSettings("AdminURL");
        string emailSubject = GetSysSettings("ForgetPasswordEmailSubject");

        var emailFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/ForgetPasswordEmail/EmailEn.html", FileMode.Open, FileAccess.Read);
        string emailBody = "";
        using (StreamReader reader = new StreamReader(emailFileStream))
        {
            emailBody = reader.ReadToEnd();
        }
        emailBody = emailBody.Replace("{{User}}", user.FullName).Replace("{{Link}}", adminURL + "/resetPassword/" + token);

        var sent = _notificationService.CreateEmailNotification(emailSubject, emailBody, null, new List<string>() { user.Email }, EnumPriority.High);

        //_emailSender.SendEmailAsync(user.Email, emailSubject, emailBody);

        result.IsSuccess = true;
        result.Data = true;
        return result;
    }

    private JwtSecurityToken DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var result = handler.ReadJwtToken(token);
        return result;
    }

    public ResultViewModel<bool> SetPassword(SetPasswordDto dto)
    {
        var result = new ResultViewModel<bool>();

        _baseService.CheckValidation(dto, _setPasswordValidator);

        var token = dto?.Token;
        if (token == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidToken", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var decodedToken = DecodeToken(token);
        var userId = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
        if (userId == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidToken", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => x.Id == int.Parse(userId) && x.IsDeleted != true);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.IsActive != true)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "msg_AccountIsDisabled", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        user.SetPassword(dto.NewPassword, _passwordHasher);
        _dbcontext.SaveChanges();

        result.IsSuccess = true;
        result.Data = true;
        return result;
    }

    public ResultViewModel<bool> ResetPassword(ResetPasswordDto dto)
    {
        var result = new ResultViewModel<bool>();

        _baseService.CheckValidation(dto, _resetPasswordValidator);

        var token = dto?.Token;
        if (token == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidToken", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var decodedToken = DecodeToken(token);
        var userId = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
        if (userId == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidToken", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        var user = _dbcontext.SupplierAppUsers.FirstOrDefault(x => x.Id == int.Parse(userId) && x.IsDeleted != true);
        if (user == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "InvalidUser", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (user.IsActive != true)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "msg_AccountIsDisabled", PropertyName = "" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        user.UpdatePassword(dto.NewPassword, _passwordHasher);
        _dbcontext.SaveChanges();

        result.IsSuccess = true;
        result.Data = true;
        return result;
    }
    private void SendCreateUserEmail(SupplierAppUser user, string otp)
    {

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

}