using Hackaton.Core.Enumerators;

namespace Hackaton.API.Security.Roles.Domain
{
    public class Role
    {
        public int Id { get; init; }
        public ERole Name { get; init; }
    }
}
