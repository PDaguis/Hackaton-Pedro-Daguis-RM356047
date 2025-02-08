using Hackaton.Core.Entities.Roles.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IUsuarioRoleRepository : IGenericRepository<UsuarioRole>
    {
        Task<List<UsuarioRole>> GetByUsuarioId(Guid usuarioId);
    }
}
