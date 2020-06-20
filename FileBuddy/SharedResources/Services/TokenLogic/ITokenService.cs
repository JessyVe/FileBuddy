using SharedRessources.Dtos;
using System.Collections.Generic;
using System.Security.Claims;

namespace SharedRessources.Services.TokenLogic
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Claim[] GetUserClaims(AppUser user);
        void GenerateTokensForUser(AppUser user);
    }
}
