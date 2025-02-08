using Hackaton.Core.Entities;
using Hackaton.Core.Entities.Roles.Domain;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace Hackaton.Infra.Repositories
{
    public class UsuarioRoleRepository : GenericRepository<UsuarioRole>, IUsuarioRoleRepository
    {
        public UsuarioRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<UsuarioRole>> GetByUsuarioId(Guid usuarioId)
        {
            var filter = Builders<UsuarioRole>.Filter.Eq(x => x.UsuarioId, usuarioId);

            return await _context.GetCollection<UsuarioRole>(typeof(UsuarioRole).Name)
                .Find(filter)
                .ToListAsync();
        }
    }
}
