using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace IServiceContractor.IdentityInterFaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);

    }
}
