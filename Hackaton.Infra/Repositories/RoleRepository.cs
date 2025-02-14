﻿using Hackaton.Core.Entities;
using Hackaton.Core.Entities.Roles.Domain;
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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByRoleId(int id)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.RoleId, id);

            return await _context.GetCollection<Role>(typeof(Role).Name)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Exists()
        {
            var filter = Builders<Role>.Filter.Or(
                Builders<Role>.Filter.Eq(d => d.Name, Core.Enumerators.ERole.Paciente),
                Builders<Role>.Filter.Eq(d => d.Name, Core.Enumerators.ERole.Medico),
                Builders<Role>.Filter.Eq(d => d.Name, Core.Enumerators.ERole.Administrador)
            );

            return await _context.GetCollection<Role>(typeof(Role).Name)
                .Find(filter)
                .AnyAsync();
        }
    }
}
