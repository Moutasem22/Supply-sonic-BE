using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SysSetting : BaseEntity<int>
    {
        public string SysKey { get; set; }
        public string SysValue { get; set; }
    }
}
