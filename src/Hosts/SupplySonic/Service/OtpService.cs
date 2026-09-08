
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class OtpService : IOtpService
    {
        private DBContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService userService;
        private readonly AppSettings _appSettings;
        public string lang { get; set; }
        public OtpService(DBContext dbcontext,  IHttpContextAccessor httpContextAccessor, IUserService _userService, IOptions<AppSettings> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbcontext = dbcontext;
           
            userService = _userService;
            _appSettings = options.Value;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";          
        }
        public Tuple<string, string> CreateCode()
        {
            ResultViewModel<string> _ResultViewModel = new ResultViewModel<string>();

            var digit = 4;

            var otp = new Random().Next(0, (int)Math.Pow(10, digit) - 1).ToString("####");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var claims = new List<Claim>();

            claims.AddRange(userService.generateOTPClaims(otp));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(int.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "AccessTokenTimeout").SysValue ?? "5")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Tuple.Create(tokenHandler.WriteToken(token), otp);
        }
    }
}
