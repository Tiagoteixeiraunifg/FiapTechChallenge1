using Biblioteca.Infraestrutura.Logs.Fabricas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Logs.Provedor
{
    public class ProvedorDeLogsCustomizado : ILoggerProvider
    {
        private readonly FabricaDeLogs _FabricaDeLogs;

        private readonly ConcurrentDictionary<string, LogCustomizado> _Logs = new ConcurrentDictionary<string, LogCustomizado>();

        public ProvedorDeLogsCustomizado(FabricaDeLogs fabricaDeLogs)
        {
            _FabricaDeLogs = fabricaDeLogs;
        }



        public ILogger CreateLogger(string categoryName)
        {
            return _Logs.GetOrAdd(categoryName, nome => new LogCustomizado(nome, _FabricaDeLogs));
        }



        public void Dispose()
        {
            this.Dispose();
        }
    }
}
