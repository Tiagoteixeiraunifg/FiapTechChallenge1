using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Logs.Fabricas
{
    public class FabricaDeLogs
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public int IdEvento { get; set; } = 0;

    }
}
