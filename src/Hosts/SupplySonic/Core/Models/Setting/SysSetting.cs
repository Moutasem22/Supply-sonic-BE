using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Setting
{
    public class SysSetting : BaseEntity<int>
    {
        public string SysKey { get; set; }
        public string SysValue { get; set; }
    }
}
