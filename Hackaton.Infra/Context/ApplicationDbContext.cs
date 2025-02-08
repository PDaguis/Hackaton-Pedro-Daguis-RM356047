using Hackaton.Shared.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Infra.Context
{
    public class ApplicationDbContext
    {
        private readonly HackatonOptions _options;
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IOptions<HackatonOptions> options)
        {
            _options = options.Value;

            var client = new MongoClient(_options.ConnectionString);
            _database = client.GetDatabase(_options.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public async Task<IClientSession> StartSessionAsync()
        {
            return await _database.Client.StartSessionAsync();
        }
    }
}
