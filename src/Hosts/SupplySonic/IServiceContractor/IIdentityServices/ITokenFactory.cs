using System;

namespace IServiceContractor.IdentityInterFaces
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
