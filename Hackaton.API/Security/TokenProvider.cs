using Hackaton.Core.Entities;
using Hackaton.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Hackaton.API.Security
{
    public class TokenProvider
    {
        private readonly HackatonOptions _hackatonOptions;

        public TokenProvider(IOptions<HackatonOptions> hackatonOptions)
        {
            _hackatonOptions = hackatonOptions.Value;
        }

        public string GenerateToken(Usuario usuario)
        {
            string secretKey = _hackatonOptions.SecretKey;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Role.ToString())
                ]),
                Expires = DateTime.UtcNow.AddSeconds(_hackatonOptions.TokenExpiration),
                SigningCredentials = credentials,
                Issuer = _hackatonOptions.Issuer,
                Audience = _hackatonOptions.Audience
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
