using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Shared.Options
{
    public class HackatonOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
