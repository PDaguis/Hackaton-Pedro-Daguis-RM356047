using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Infra.Repositories
{
    public class ConsultaRepository : GenericRepository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Consulta>> GetAllByStatusMedico(EStatusConsulta status, Guid medicoId)
        {
            var filter = Builders<Consulta>.Filter.And(
                Builders<Consulta>.Filter.Eq(d => d.MedicoId, medicoId),
                Builders<Consulta>.Filter.Where(x => x.StatusConsulta.Last().Status.Equals(status))
            );

            return await _context.GetCollection<Consulta>(typeof(Consulta).Name)
                .Find(filter)
                .ToListAsync();
        }
    }
}
