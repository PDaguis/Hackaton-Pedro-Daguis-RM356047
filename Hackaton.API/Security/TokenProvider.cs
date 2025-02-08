using Hackaton.Core.Entities;
using Hackaton.Core.Entities.Roles.Domain;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Hackaton.API.Security
{
    public class TokenProvider
    {
        private readonly HackatonOptions _hackatonOptions;
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public TokenProvider(IOptions<HackatonOptions> hackatonOptions, IUsuarioRoleRepository usuarioRoleRepository, IRoleRepository roleRepository)
        {
            _hackatonOptions = hackatonOptions.Value;
            _usuarioRoleRepository = usuarioRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<string> GenerateToken(Usuario usuario)
        {
            string secretKey = _hackatonOptions.SecretKey;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<UsuarioRole> rolesNome = await _usuarioRoleRepository.GetByUsuarioId(usuario.Id);

            List<Claim> claims = [
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
                            ];

            foreach (var item in rolesNome.Select(x => x.RoleId))
            {
                var role = await _roleRepository.GetByRoleId(item);
                claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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
