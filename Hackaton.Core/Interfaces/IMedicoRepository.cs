using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IMedicoRepository : IGenericRepository<Medico>
    {
        Task<IEnumerable<Medico>> ObterPorEspecialidade(EEspecialidade especialidade);
        Task<Medico> ObterPorCrm(string crm);
        Task<Medico> ObterPorNome(string nome);
        Task<IEnumerable<EEspecialidade>> ListarEspecialidades();
    }
}
