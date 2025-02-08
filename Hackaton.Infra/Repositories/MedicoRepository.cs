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
    public class MedicoRepository : GenericRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<EEspecialidade>> ListarEspecialidades()
        {
            return Task.FromResult(Enum.GetValues<EEspecialidade>().AsEnumerable());
        }

        public Task<Medico> ObterPorCrm(string crm)
        {
            var filter = Builders<Medico>.Filter.Eq(x => x.Crm, crm);

            return _context.GetCollection<Medico>(typeof(Medico).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Medico>> ObterPorEspecialidade(EEspecialidade especialidade)
        {
            var filter = Builders<Medico>.Filter.Eq(x => x.Especialidade, especialidade);

            return await _context.GetCollection<Medico>(typeof(Medico).Name)
                .Find(filter)
                .ToListAsync();
        }

        public Task<Medico> ObterPorNome(string nome)
        {
            var filter = Builders<Medico>.Filter.Eq(x => x.Nome, nome);

            return _context.GetCollection<Medico>(typeof(Medico).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }
    }
}
