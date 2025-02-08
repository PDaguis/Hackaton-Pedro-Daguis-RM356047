using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> ObterPorEmail(string email);
        Task<Usuario> ObterPorNome(string nome);
        Task<Usuario> ObterPorDocumento(string documento);
        Task<IEnumerable<Usuario>> Listar(ERole role);

        #region Medico
        Task<Medico> ObterMedicoPorId(Guid id);
        Task<Medico> ObterPorCrm(string crm);
        Task<IEnumerable<Medico>> ObterPorEspecialidade(EEspecialidade especialidade);
        #endregion

        #region Paciente
        Task<Paciente> ObterPacientePorId(Guid id);
        #endregion
    }
}
