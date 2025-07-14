
using Core.Models;
using DB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapster;
using Core.Models.Setting;
using DTO.SettingDTO;
using IServiceContractor.ISettingServices;

namespace Service.SettingServices
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private DBContext _dbcontext;
        public string lang { get; set; }
        public UserSettingService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;

            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";
        }

        public List<UserSettingDto> GetUserSetting()
        {
            int cuserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value);
            var userSettings = _dbcontext.UserSettings.Where(s => s.UserId == cuserId);
            var userSettingDtos = userSettings.Adapt<List<UserSettingDto>>();//_mapper.Map<List<UserSettingDto>>(userSettings);
            return userSettingDtos;
        }

        public string SetUserSetting(string key, string value)
        {
            int cuserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value);
            var userSetting = _dbcontext.UserSettings.FirstOrDefault(s => s.UserId == cuserId && s.Key.ToLower() == key.ToLower());
            if (userSetting != null)
            {
                userSetting.Update(value);
                _dbcontext.SaveChanges();
                return userSetting.Value;
            }
            userSetting = new UserSetting(cuserId, key, value);
            _dbcontext.UserSettings.Add(userSetting);
            _dbcontext.SaveChanges();
            return userSetting.Value;
        }
    }
}
