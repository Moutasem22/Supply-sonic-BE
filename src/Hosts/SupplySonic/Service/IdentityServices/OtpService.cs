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

public class OtpService : IOtpService
{
    private DBContext _dbcontext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppSettings _appSettings;
    public string lang { get; set; }
    public OtpService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbcontext = dbcontext;
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

        claims.AddRange(generateOTPClaims(otp));
        var accesstokentime = _dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "AccessTokenTimeout").SysValue;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.Now.AddMinutes(int.Parse(accesstokentime ?? "5")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Tuple.Create(tokenHandler.WriteToken(token), otp);
    }


    #region Helpers Funactions  
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
    string ComputeStringToSha256Hash(string plainText)
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

    #endregion



}
