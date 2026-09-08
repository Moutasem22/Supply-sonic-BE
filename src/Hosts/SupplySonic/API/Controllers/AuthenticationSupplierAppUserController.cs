using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Service;
namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationSupplierUserController : ControllerBase
{
    private readonly IAuthenticationSupplierAppUserService _authenticationRep;
    private readonly GlobalFormat _globalFormat;
    public AuthenticationSupplierUserController(IAuthenticationSupplierAppUserService authenticationRep, GlobalFormat globalFormat)
    {
        _authenticationRep = authenticationRep;
        _globalFormat = globalFormat;
    }
    [HttpPost("login")]
    public ActionResult<ResultViewModel<UserInfo>> Login([FromBody] LoginModelDTO model)
    {
        var lang = string.IsNullOrWhiteSpace(HttpContext.Request.Headers["lang"]) ? "ar" : HttpContext.Request.Headers["lang"].ToString();
        var result = _authenticationRep.Authenticate(model.UserName, model.Password, model.RemoteIpAddress);
        return Ok(result);
    }
    [HttpGet("ResendOTP/{UserId}")]
    [AllowAnonymous]
   // [Authorize(Policy = "LoginOtp")]
    public IActionResult ResendOTP(int UserId)
    {
        //var userId = HttpContext.User.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value;
        var val = _authenticationRep.ResendOTP(UserId);
        return Ok(val);
    }
   
    [HttpPost("Verify")]
   // [Authorize(Policy = "LoginOtp")]
    public ActionResult<ResultViewModel<UserInfo>> Verify([FromBody] VerifyDto model)
    {
        var result = _authenticationRep.Verify(model);
        return Ok(result);
    }

    [HttpGet("Logout")]
    //[Authorize(Policy = "Verified")]
    public ActionResult<ResultViewModel<bool>> Logout()
    {
        var claims = HttpContext.User.Claims;
        var lang = string.IsNullOrWhiteSpace(HttpContext.Request.Headers["lang"]) ? "ar" : HttpContext.Request.Headers["lang"].ToString();
        var result = _authenticationRep.Logout(claims);
        return Ok(result);
    }
    [HttpPost("GetAccessTokenUsingRefreshToken")]
    public ActionResult<ResultViewModel<UserInfo>> GetAccessTokenUsingRefreshToken([FromBody] RegenerateTokenDTO model)
    {

        var lang = string.IsNullOrWhiteSpace(HttpContext.Request.Headers["lang"]) ? "ar" : HttpContext.Request.Headers["lang"].ToString();
        var result = _authenticationRep.GetAccessTokenUsingRefreshToken(model.OldAccessToken, model.RefreshToken);
        return Ok(result);
    }

    [HttpGet("Authorize")]
    [Authorize]
    public ActionResult Authorize(string currentPageCode = "")
    {
        var accessToken = string.Empty;
        List<Tuple<string, string>> Headers = null;
        if (Request.Headers.ContainsKey("Authorization"))
        {
            accessToken = Request.Headers["Authorization"];
            Headers = new List<Tuple<string, string>>() { Tuple.Create<string, string>("Authorization", accessToken) };
        }

        int userid = int.Parse(HttpContext.User.Claims.First(c => c.Type == "UserId").Value);
        var res = _authenticationRep.UserInfoAndPages(userid, currentPageCode);

        var edata = _globalFormat.LogFormat(HttpContext, res);

        return Ok(res);

    }

    [HttpGet("TokenCheck")]//used for web
    [Authorize]
    public ActionResult TokenCheck()
    {


        var accessToken = string.Empty;
        List<Tuple<string, string>> Headers = null;
        if (Request.Headers.ContainsKey("Authorization"))
        {
            accessToken = Request.Headers["Authorization"];
            Headers = new List<Tuple<string, string>>() { Tuple.Create<string, string>("Authorization", accessToken) };
        }

        int userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == "UserId").Value);
        var userInfo = _authenticationRep.getItem(userId);

        return Ok(userInfo);

    }

    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<ActionResult> ChangePassword(PasswordDto dto)
    {
        var result = await _authenticationRep.ChangePassword(dto);
        return Ok(result);
    }

    [HttpPost("ForgetPassword")]
    public ActionResult ForgetPassword(ForgetPasswordDto model)
    {
        var result = _authenticationRep.ForgetPassword(model.Email);
        return Ok(result);
    }

    [HttpPost("SetPassword")]
    //[Authorize(Policy = "SetPassword")]
    public ActionResult SetPassword(SetPasswordDto model)
    {
        var result = _authenticationRep.SetPassword(model);
        return Ok(result);
    }


    [HttpPost("ResetPassword")]
    //[Authorize(Policy = "ForgetPassword")]
    public ActionResult ResetPassword(ResetPasswordDto model)
    {
        var result = _authenticationRep.ResetPassword(model);
        return Ok(result);
    }
}
