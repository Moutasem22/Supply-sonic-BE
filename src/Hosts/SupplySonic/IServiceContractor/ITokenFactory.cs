using System;

namespace IServiceContractor
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
