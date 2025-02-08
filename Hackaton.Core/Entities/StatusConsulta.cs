using Hackaton.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public class StatusConsulta
    {
        public EStatusConsulta Status { get; set; }
        public DateTime CadastradoEm { get; private set; }

        public StatusConsulta()
        {
            CadastradoEm = DateTime.Now;
        }
    }
}
