using Hackaton.Core.Enumerators;

namespace Hackaton.Core.Entities.Roles.Domain
{
    public class Role : EntityBase
    {
        public const int PacienteId = 1;
        public const int MedicoId = 2;
        public const int Administrador = 3;

        public int RoleId { get; set; }
        public ERole Name { get; set; }
    }
}
