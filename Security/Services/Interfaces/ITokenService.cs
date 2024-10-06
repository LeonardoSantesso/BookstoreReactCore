
using System.Security.Claims;

namespace Security.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IList<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}