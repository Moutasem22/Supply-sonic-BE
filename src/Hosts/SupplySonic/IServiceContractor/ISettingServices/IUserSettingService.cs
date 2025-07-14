using DTO.SettingDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.ISettingServices
{
    public interface IUserSettingService
    {
        List<UserSettingDto> GetUserSetting();
        string SetUserSetting(string key, string value);

    }
}
