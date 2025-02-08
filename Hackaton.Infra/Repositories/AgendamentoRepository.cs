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
    public class AgendamentoRepository : GenericRepository<Agendamento>, IAgendamentoRepository
    {
        public AgendamentoRepository(ApplicationDbContext context) : base(context)
        { }

        public async Task<Agendamento> ObterDisponivel(Guid id)
        {
            var filter = Builders<Agendamento>.Filter.And(
                Builders<Agendamento>.Filter.Eq(d => d.Id, id),
                Builders<Agendamento>.Filter.Gte(d => d.Disponivel, true)
            );

            return await _context.GetCollection<Agendamento>(typeof(Agendamento).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Agendamento>> ObterHorariosDisponiveisPorMedico(Guid medicoId, DateTime dataInicio, DateTime dataFinal)
        {
            var filter = Builders<Agendamento>.Filter.And(
                Builders<Agendamento>.Filter.Eq(d => d.MedicoId, medicoId),
                Builders<Agendamento>.Filter.Gte(d => d.DataHora, dataInicio),
                Builders<Agendamento>.Filter.Lte(d => d.DataHora, dataFinal),
                Builders<Agendamento>.Filter.Eq(d => d.Disponivel, true)
            );

            return await _context.GetCollection<Agendamento>(typeof(Agendamento).Name)
                .Find(filter)
                .SortBy(x => x.DataHora)
                .ToListAsync();
        }

        public async Task<Agendamento> ObterPorData(DateTime data)
        {
            var session = await _context.StartSessionAsync();

            var filter = Builders<Agendamento>.Filter.Gte(x => x.DataHora, data);

            return await _context.GetCollection<Agendamento>(typeof(Agendamento).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }
    }
}
