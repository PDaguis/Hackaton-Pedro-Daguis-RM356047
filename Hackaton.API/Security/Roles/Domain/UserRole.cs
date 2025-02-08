using Hackaton.Core.Entities;

namespace Hackaton.API.Security.Roles.Domain
{
    public class UserRole
    {
        public Guid UsuarioId { get; set; }
        public int RoleId { get; set; }
        public Usuario Usuario { get; set; }
        public Role Role { get; set; }
    }
}
