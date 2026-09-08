using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Models.Identity;

public class SupplierAppUser : IdentityUser<int>
{
    public SupplierAppUser()
    {

    }



    private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();


    public int? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public int? ModifiedBy { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; }

    public string UserName { get; private set; }
    public string FullName { get; private set; }
    public string? VerificationCode { get; private set; }
    public bool? IsEnabled { get; private set; }
    public bool? IsConfirmed { get; private set; }
    public string? Token { get; set; }

    public bool IsSuperAdmin { get; private set; }


    public string? resetPasswordCode { get; private set; }
    public DateTime? resetPasswordCodeTimeOut { get; private set; }
    public int UserType { get; private set; } = 0;
    public EnumUserCategory? UserCategory { get; private set; } = 0;
    public string? UserId { get; private set; }


    public string? Extension { get; set; }
    public int? ProfileAttachmentId { get; set; }
    public Attachment ProfileAttachment { get; set; }

    [NotMapped]
    public string? Password { get; private set; }

    //New Fileds For 
    public int? ResidenceCountryId { get; private set; }
    public Country ResidenceCountry { get; private set; }

    public int? CityId { get; private set; }
    public City City { get; private set; }


    public string? Address { get; private set; }
    public DateTime? Birthdate { get; private set; }
    public EnumGender? Gender { get; private set; }

    public string? OtpCode { get; private set; }
    public DateTime? OtpDate { get; set; }

    public bool? IsSupplier { get; private set; } = false;
    public EnumStatus? Status { get; private set; } = EnumStatus.Pending;

    public string? RejectedReason { get; set; }
    public SupplierAppUser(string username, string fullName, string email, string otpCod, DateTime? otpDate, string userId, bool isSupplier, int? profileAttachmentId = null, EnumUserCategory userCategory = EnumUserCategory.InternalUser,
     bool isActive = true, int id = 0) : this()
    {
        this.UserName = username;
        this.FullName = fullName;
        this.Email = email;
        this.Id = id;
        this.UserId = "000";
        this.UserCategory = userCategory;
        this.IsActive = isActive;
        this.EmailConfirmed = false;
        this.ProfileAttachmentId = profileAttachmentId;
        this.OtpCode = otpCod;

        this.OtpDate = otpDate;
        this.IsSupplier = isSupplier;
        this.Status = EnumStatus.Pending;

    }
    public void Update(string userName, string fullName, string email, string userId, string Password, IPasswordHasher<SupplierAppUser> _passwordHasher
        , EnumUserCategory userCategory = EnumUserCategory.InternalUser, bool isActive = true)
    {
        if (!string.IsNullOrWhiteSpace(Password))
        {
            var passwordHash = _passwordHasher.HashPassword(this, Password);
            this.PasswordHash = passwordHash;
        }
        this.UserId = userId;
        this.UserName = userName;
        this.FullName = fullName;
        this.Email = email;


        this.UserCategory = userCategory;
        this.IsActive = isActive;



    }



    public void UpdateName(string fullName)
    {
        this.FullName = fullName;
    }

    public void UpdateProfile(string phoneNumber, int? profileAttachmentId, string fullName, int? countryId, int? cityId, string? address)
    {
        this.PhoneNumber = phoneNumber;
        this.ProfileAttachmentId = profileAttachmentId;
        this.CityId = cityId;
        this.ResidenceCountryId = countryId;
        this.Address = address;
    }

    public void SetPassword(string newPassword, IPasswordHasher<SupplierAppUser> _passwordHasher)
    {
        var passwordHash = _passwordHasher.HashPassword(this, newPassword);
        this.PasswordHash = passwordHash;
    }
    public void UpdatePassword(string newPassword, IPasswordHasher<SupplierAppUser> _passwordHasher)
    {
        var passwordHash = _passwordHasher.HashPassword(this, newPassword);
        this.PasswordHash = passwordHash;
    }

    ///https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
    public void AddRereshToken(string token, string remoteIpAddress, double MinutesToExpire = 5)
    {
        _refreshTokens.Add(new RefreshToken(token, DateTime.Now.AddMinutes(MinutesToExpire), this.Id, remoteIpAddress, 2));
    }
    public bool HasValidRefreshToken(string refreshToken)
    {
        return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.IsActive && rt.IsDeleted != true && rt.Expires >= DateTime.Now);
    }
    public void RemoveAllRefreshToken()
    {
        _refreshTokens.RemoveAll(x => x.IsActive && x.Expires < DateTime.Now);
    }
    public void RemoveRefreshToken(string refreshToken)
    {
        _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
    }
    public void UpdateUserType(int newUserType)
    {
        this.UserType = newUserType;
    }


    public void UpdateToken(string newToken)
    {
        this.Token = newToken;
        this.EmailConfirmed = true;
        this.IsConfirmed = true;
        this.IsEnabled = true;
    }


    public virtual void Delete()
    {
        this.IsDeleted = true;
    }
    public virtual void UndoDelete()
    {
        this.IsDeleted = false;
    }
    public virtual void Activate()
    {
        this.IsActive = true;
    }
    public virtual void Deactivate()
    {
        this.IsActive = false;
    }
    public virtual void LogOut()
    {
        this.Token = null;
    }

    public void UpdateAddress(int? countryId, int? cityId, string phone, string address)
    {

        this.CityId = cityId;
        this.ResidenceCountryId = countryId;
        this.PhoneNumber = phone;
        this.Address = address;
        this.UserId = RandamUserId();
    }

    private string RandamUserId()
    {
        Random r = new Random();
        int rInt = r.Next(1, 5);
        return (this.Id + rInt).ToString();

    }


    public void SendOtp(string otpCod, DateTime? otpDate)
    {

        this.OtpCode = otpCod;

        this.OtpDate = otpDate;
    }

    public void UpdateStatus(EnumStatus? status, string? rejectedReason)
    {
        this.Status = status;
        this.RejectedReason = rejectedReason;
    }

}
