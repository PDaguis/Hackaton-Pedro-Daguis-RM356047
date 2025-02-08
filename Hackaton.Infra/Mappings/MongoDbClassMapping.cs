using Hackaton.Core.Entities;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Infra.Mappings
{
    public static class MongoDbClassMapping
    {
        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Usuario)))
            {
                BsonClassMap.RegisterClassMap<Usuario>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.AddKnownType(typeof(Paciente));
                    cm.AddKnownType(typeof(Medico));
                    cm.SetDiscriminatorIsRequired(true);
                    cm.SetDiscriminator("Usuario");
                });

                BsonClassMap.RegisterClassMap<Paciente>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Paciente");
                });

                BsonClassMap.RegisterClassMap<Medico>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("Medico");
                });
            }
        }
    }
}
