using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IConsultaRepository : IGenericRepository<Consulta>
    {
        Task<IEnumerable<Consulta>> GetAllByStatusMedico(EStatusConsulta status, Guid medicoId);
    }
}
