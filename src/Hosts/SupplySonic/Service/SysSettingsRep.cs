using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class SysSettingsRep
    {
        private DBContext _dbcontext;
        public SysSettingsRep(DBContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public string GetAppSetting(string key)
        {
            var value = _dbcontext.SysSettings.FirstOrDefault(s => s.SysKey.ToLower() == key.ToLower())?.SysValue;
            return value ?? "";
        }
    }
}
