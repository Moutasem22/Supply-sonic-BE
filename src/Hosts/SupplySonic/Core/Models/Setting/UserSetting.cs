using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Setting
{
    public class UserSetting : BaseEntity<int>
    {
        private UserSetting()
        {

        }
        public UserSetting(int userId, string key, string value)
        {
            UserId = userId;
            Key = key;
            Value = value;
        }

        public void Update(string value)
        {
            Value = value;
        }
        public int UserId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
