using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.IdentityInterFaces
{
    public interface IOtpService
    {
        public Tuple<string, string> CreateCode();

    }
}
