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
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Login(string email)
        {
            try
            {
                var filter = Builders<Usuario>.Filter.Eq(x => x.Email, email);

                return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                    .Find(filter)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
