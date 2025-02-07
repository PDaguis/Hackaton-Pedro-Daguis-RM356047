using Hackaton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Interfaces
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task Cadastrar(T entidade);
        Task Atualizar(T entidade);
        Task Excluir(Guid id);
        Task ExcluirTudo();
    }
}
