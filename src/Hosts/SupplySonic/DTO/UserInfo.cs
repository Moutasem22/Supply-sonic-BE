using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace DTO
{
    public class UserInfo
    {
        public ClaimsIdentity claimsIdentity { get; set; }
        public UserDTO AppUsers { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string OtpCode { get; set; }
        public int OtpTimeOut { get; set; }
        public string Lang { get; set; }
        public bool? NeedOTP { get; set; }
        public bool? IsSupplier { get; set; }
        public EnumStatus? Status { get; set; }

    }
    public class LoginModelDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteIpAddress { get; set; }

    }

    public class VerifyDto
    {
        public string VerificationCode { get; set; }
        public string RemoteIpAddress { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
    }

    public class ForgetPasswordDto
    {
        public string Email { get; set; }
        public string RemoteIpAddress { get; set; }
    }

    public class SetPasswordDto
    {
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public string RemoteIpAddress { get; set; }
    }

    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
        //public string Token { get; set; }
        public string RemoteIpAddress { get; set; }
        public string Token { get; set; }
    }

    public class RegenerateTokenDTO
    {
        public string OldAccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class ExtrnalToken
    {
        public string Token { get; set; }
        public int TokenDuration { get; set; }
    }
}
