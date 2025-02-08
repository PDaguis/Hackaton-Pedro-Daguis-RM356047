using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Infra.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Usuario>> Listar(ERole role)
        {
            try
            {
                var filter = Builders<Usuario>.Filter.Eq(x => x.Role, role);

                return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                    .Find(filter)
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Medico?> ObterMedicoPorId(Guid id)
        {
            try
            {
                var filter = Builders<Medico>.Filter.And(
                    Builders<Medico>.Filter.Eq(u => u.Id, id),
                    Builders<Medico>.Filter.Eq(u => u.Role, ERole.Medico)
                );

                return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                    .OfType<Medico>()
                    .Find(filter)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Paciente?> ObterPacientePorId(Guid id)
        {
            try
            {
                var filter = Builders<Paciente>.Filter.And(
                    Builders<Paciente>.Filter.Eq(u => u.Id, id),
                    Builders<Paciente>.Filter.Eq(u => u.Role, ERole.Paciente)
                );

                return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                    .OfType<Paciente>()
                    .Find(filter)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Medico?> ObterPorCrm(string crm)
        {
            try
            {
                var filter = Builders<Medico>.Filter.And(
                    Builders<Medico>.Filter.Eq(u => u.Crm, crm),
                    Builders<Medico>.Filter.Eq(u => u.Role, ERole.Medico)
                );

                return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                    .OfType<Medico>()
                    .Find(filter)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Usuario> ObterPorDocumento(string documento)
        {
            var medicoFilter = Builders<Medico>.Filter.Eq(u => u.Crm, documento);
            var pacienteFilter = Builders<Paciente>.Filter.Or(
                Builders<Paciente>.Filter.Eq(u => u.Cpf, documento),
                Builders<Paciente>.Filter.Eq(u => u.Email, documento)
            );
            var adminFilter = Builders<Administrador>.Filter.Eq(u => u.Email, documento);

            var usuarioFilter = Builders<Usuario>.Filter.Or(
                Builders<Usuario>.Filter.And(
                    Builders<Usuario>.Filter.Eq(u => u.Role, ERole.Medico),
                    Builders<Usuario>.Filter.Eq(u => ((Medico)u).Crm, documento)
                ),
                Builders<Usuario>.Filter.And(
                    Builders<Usuario>.Filter.Eq(u => u.Role, ERole.Paciente),
                    Builders<Usuario>.Filter.Or(
                        Builders<Usuario>.Filter.Eq(u => ((Paciente)u).Cpf, documento),
                        Builders<Usuario>.Filter.Eq(u => ((Paciente)u).Email, documento)
                    )
                ),
                Builders<Usuario>.Filter.And(
                    Builders<Usuario>.Filter.Eq(u => u.Role, ERole.Administrador),
                    Builders<Usuario>.Filter.Eq(u => ((Administrador)u).Email, documento)
                )
            );

            return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                .Find(usuarioFilter)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorEmail(string email)
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

        public async Task<IEnumerable<Medico?>> ObterPorEspecialidade(EEspecialidade especialidade)
        {
            var filter = Builders<Medico>.Filter.Eq(x => x.Especialidade, especialidade);

            return await _context.GetCollection<Usuario>(typeof(Usuario).Name)
                .OfType<Medico>()
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Usuario> ObterPorNome(string nome)
        {
            try
            {
                var filter = Builders<Usuario>.Filter.Eq(x => x.Nome, nome);

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
