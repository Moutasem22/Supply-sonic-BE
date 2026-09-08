
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;

namespace Core.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
            UserRoles = new HashSet<UserRole>();
           

        }
        /// <summary>
        /// لإدخال مستخدم جديد
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="organizationalStructureId"></param>
        /// <param name="titleId"></param>
        /// <param name="userType"></param>
        /// <param name="id"></param>


        public AppUser(string username, string fullName, string email, string userId, List<UserRole> userRoles
             , string? phone, string? address,
             EnumGender? gender, DateTime? birthday, int? residenceCountryId
                    , int? cityId, string? stateName,
            EnumUserCategory userCategory = EnumUserCategory.InternalUser,
           
            bool isActive = true, int id = 0) : this()
        {
            this.UserName = username;
            this.FullName = fullName;
            this.Email = email;
            this.Id = id;
            this.UserId = userId;
            this.UserCategory = userCategory;        
            this.IsActive = isActive;
            this.UserRoles = userRoles;
            this.PhoneNumber = phone;
            this.Address = address;
            this.Gender = gender;
            this.Birthdate = birthday;
            this.ResidenceCountryId = residenceCountryId;
            this.CityId = cityId;
            this.StateName = stateName;
        }
        public void Update(string userName, string fullName, string email, string userId, string Password, IPasswordHasher<AppUser> _passwordHasher, List<int> userRoles
            ,string? phone, string? address,
               EnumGender? gender, DateTime? birthday, int? residenceCountryId
                    , int? cityId, string? stateName,
            EnumUserCategory userCategory = EnumUserCategory.InternalUser,  bool isActive = true 
         
            )
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
            this.Address = address;

            this.UserCategory = userCategory;
            this.IsActive = isActive;
            this.PhoneNumber = phone;
            this.Gender = gender;
            this.Birthdate = birthday;
            this.ResidenceCountryId = residenceCountryId;
            this.CityId = cityId;
            this.StateName = stateName;



            //To delete items 
            List<int> deletedIds = this.UserRoles.Select(x => x.RoleId).Except(userRoles).ToList();

            var deletedItems = this.UserRoles.Where(x => deletedIds.Contains(x.RoleId)).ToList();
            foreach (var item in deletedItems)
            {
                this.UserRoles.Remove(item);
            }

            //Save New  Items
            List<int> addedItem = userRoles.Except(this.UserRoles.Select(x => x.RoleId)).ToList();

            foreach (var item in addedItem)
            {
                this.UserRoles.Add(new UserRole() { RoleId = item });
            }



        }
        public void UpdateName(string fullName)
        {
            this.FullName = fullName;
        }

        public void UpdateProfile(string phoneNumber, string extention, int? profileAttachmentId)
        {
            this.PhoneNumber = phoneNumber;
            this.Extension = extention;
            this.ProfileAttachmentId = profileAttachmentId;
        }

        public void SetPassword(string newPassword, IPasswordHasher<AppUser> _passwordHasher)
        {
            var passwordHash = _passwordHasher.HashPassword(this, newPassword);
            this.PasswordHash = passwordHash;
        }
        public void UpdatePassword(string newPassword, IPasswordHasher<AppUser> _passwordHasher)
        {
            var passwordHash = _passwordHasher.HashPassword(this, newPassword);
            this.PasswordHash = passwordHash;
        }
 
        ///https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
        public void AddRereshToken(string token, string remoteIpAddress, double MinutesToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.Now.AddMinutes(MinutesToExpire), this.Id, remoteIpAddress));
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
        }

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();


        public virtual ICollection<UserRole> UserRoles { get; private set; }



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
        public string? Token { get; private set; }
        
        public bool IsSuperAdmin { get; private set; }
 

        public string? resetPasswordCode { get; private set; }
        public DateTime? resetPasswordCodeTimeOut { get; private set; }
        public int UserType { get; private set; } = 0;
        public EnumUserCategory? UserCategory { get; private set; } = 0;
        public string? UserId { get; private set; }

  
        public string? Extension { get; set; }
        public int? ProfileAttachmentId { get; set; }
        public Attachment ProfileAttachment { get; set; }
        
        public string? Address { get; private set; }

        //New Fileds For 
        public int? ResidenceCountryId { get; private set; }
        public Country ResidenceCountry { get; private set; }

        public int? CityId { get; private set; }
        public City City { get; private set; }

        public string? StateName { get; private set; }
        public DateTime? Birthdate { get; private set; }
        public EnumGender? Gender { get; set; }

        [NotMapped]
        public string? Password { get; private set; }

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
        public virtual void RecoveryUser()
        {
            this.IsDeleted = false;
        }
        
    }
}
