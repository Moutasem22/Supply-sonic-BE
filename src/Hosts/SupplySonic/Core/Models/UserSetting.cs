using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserSetting : BaseEntity<int>
    {
        private UserSetting()
        {

        }
        public UserSetting(int userId, string key, string value)
        {
            this.UserId = userId;
            this.Key = key;
            this.Value = value;
        }

        public void Update(string value)
        {
            this.Value = value;
        }
        public int UserId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
