using DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IAuthenticationSupplierAppUserService
{
    ResultViewModel<UserInfo> Authenticate(string username, string password, string RemoteIpAddress);
    ResultViewModel<UserInfo> Verify(VerifyDto verifyDto);
    ResultViewModel<bool> Logout(IEnumerable<Claim> claims);
    ResultViewModel<UserInfo> GetAccessTokenUsingRefreshToken(string OldAccessToken, string RefreshToken);
    UserWithPagesDTO UserInfoAndPages(int userId, string currentPageCode = "");
    UserDTO getItem(int Id);
    ResultViewModel<UserInfo> ResendOTP(int? userId);
    ExtrnalToken AuthenticateExternalSystem(string username, string password);
    Claim[] generateOTPClaims(string otp);
    public Task<ResultViewModel<bool>> ChangePassword(PasswordDto dto);
    ResultViewModel<bool> ForgetPassword(string email);
    ResultViewModel<bool> SetPassword(SetPasswordDto model);
    ResultViewModel<bool> ResetPassword(ResetPasswordDto model);
    string ComputeStringToSha256Hash(string plainText);
    bool VerifyOtp(string otpId, string otp, string otpHash);
    ResultViewModel<List<UserResultDto>> GetAllUsersForNotification(QueryViewModel<UserResultDto> queryViewModel);

    bool CheckUserIfSuperAdmin(string UserId);
}
