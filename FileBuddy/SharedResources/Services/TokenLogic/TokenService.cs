using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SharedRessources.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace SharedRessources.Services.TokenLogic
{
    public class TokenService : ITokenService
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.SecretKey));

            var jwtToken = new JwtSecurityToken(issuer: JWTConstants.Issuer,
                audience: "Anyone",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10), 
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = GetTokenValidationParametersObject();

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public TokenValidationParameters GetTokenValidationParametersObject()
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.SecretKey)),
                ValidateLifetime = false
            };
            return tokenValidationParameters;
        }

        public void GenerateTokensForUser(AppUser user)
        {
            var usersClaims = GetUserClaims(user);

            var jwtToken = GenerateAccessToken(usersClaims);
            var userRefreshToken = GenerateRefreshToken();

            user.RefreshToken = userRefreshToken;
            user.AccessToken = jwtToken;
        }

        public Claim[] GetUserClaims(AppUser user)
        {
            return new[] { new Claim(ClaimTypes.Email, user.MailAddress), new Claim(ClaimTypes.GivenName, user.Id.ToString()) };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}