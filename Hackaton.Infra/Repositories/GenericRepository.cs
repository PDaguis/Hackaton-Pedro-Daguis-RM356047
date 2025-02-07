using Hackaton.Core.Entities;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Atualizar(T entidade)
        {
            var filter = FilterId(entidade.Id);

            await _context.GetCollection<T>(typeof(T).Name)
                .ReplaceOneAsync(filter, entidade);
        }

        public async Task Cadastrar(T entidade)
        {
            await _context.GetCollection<T>(typeof(T).Name)
                .InsertOneAsync(entidade);
        }

        public async Task Excluir(Guid id)
        {
            var filter = FilterId(id);

            await _context.GetCollection<T>(typeof(T).Name)
                .DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.GetCollection<T>(typeof(T).Name)
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            var filter = FilterId(id);

            return await _context.GetCollection<T>(typeof(T).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public FilterDefinition<T> FilterId(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);

            return filter;
        }
    }
}
