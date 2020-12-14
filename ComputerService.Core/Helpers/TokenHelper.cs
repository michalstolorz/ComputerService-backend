using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ComputerService.Core.Helpers
{
    public static class TokenHelper
    {
        public static TokenValidationParameters GetTokenValidationParameters(SecurityKey key)
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = key,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateActor = false,
                ValidateIssuer = false
            };
        }

        public static RsaSecurityKey BuildRsaSigningKey(string xml)
        {
            var rsaProvider = new RSACryptoServiceProvider(2048);
            rsaProvider.FromXmlString(xml);
            var key = new RsaSecurityKey(rsaProvider);
            return key;
        }

        public static string GenerateToken(int userId, /*IList<string> userRoles, IEnumerable<Claim> permissions,*/ RsaSecurityKey key, IDateTimeProvider dateTimeProvider)
        {
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            /*claims.AddRange(permissions);*/

            /*foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }*/
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = dateTimeProvider.GetDateTimeNow().AddHours(24),
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler
            {
                SetDefaultTimesOnTokenCreation = false
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
