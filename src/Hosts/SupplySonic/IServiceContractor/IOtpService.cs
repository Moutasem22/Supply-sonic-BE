using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor
{
    public interface IOtpService
    {
        public Tuple<string, string> CreateCode();

    }
}
