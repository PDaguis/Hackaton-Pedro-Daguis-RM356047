using Hackaton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IAgendamentoRepository : IGenericRepository<Agendamento>
    {
        Task<Agendamento> ObterPorData(DateTime data);
        Task<IEnumerable<Agendamento>> ObterHorariosDisponiveisPorMedico(Guid medicoId, DateTime dataInicio, DateTime dataFinal);
        Task<Agendamento> ObterDisponivel(Guid id);
    }
}
