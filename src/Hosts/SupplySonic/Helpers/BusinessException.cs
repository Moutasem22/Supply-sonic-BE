using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class BusinessException : Exception
    {
        public BusinessException(string val) : base(val)
        {

        }
        public BusinessException(object val):base (Newtonsoft.Json.JsonConvert.SerializeObject(val))
        {

        }
    }
}