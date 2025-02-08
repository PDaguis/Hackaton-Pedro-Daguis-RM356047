using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;

namespace Hackaton.Core.Entities.Roles.Domain
{
    public class UsuarioRole : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public int RoleId { get; set; }
        public Usuario Usuario { get; set; }
        public Role Role { get; set; }
    }
}
