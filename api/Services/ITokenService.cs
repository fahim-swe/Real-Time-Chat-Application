using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;

namespace api.Services
{
    public interface ITokenService
    {
        UserTokenDTO CreateToken(User user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
        UserTokenDTO CreateToken(List<Claim> authClaims, string username);
    }
}