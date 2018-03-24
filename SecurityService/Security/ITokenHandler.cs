using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecurityService.Security
{
    public enum TokenStatus
    {
        Expired,
        Invalid,
        Damaged,
        Valid
    }
    public interface ITokenHandler<T> where T: SecurityToken
    {
        T GenerateToken(List<Claim> parametrs);
        T VerifyToken(string inputToken);
    }
}
