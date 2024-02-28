using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Logs.Fabricas
{
    public class LogCustomizadoGenerico<T> : ILogger<T>
        where T : class
    {

        private readonly string _NomeDoLog;
        private readonly FabricaDeLogs _FabricaDeLogs;

        public LogCustomizadoGenerico(string nomeDoLog, FabricaDeLogs fabricaDeLogs)
        {
            _NomeDoLog = nomeDoLog;
            _FabricaDeLogs = fabricaDeLogs;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var mensagem = String.Format($"{logLevel}: {eventId} " +
                $"- {formatter(state, exception)}");

            CrieArquivoDeLogNoRaizDaAplicacao(mensagem);
        }

        private void CrieArquivoDeLogNoRaizDaAplicacao(string mensagem)
        {
            var pathAbsoluto = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
            var nomeArquivo = @$"{pathAbsoluto}\LOG-{DateTime.Now.Date.ToLongDateString()}.txt";

            if (!File.Exists(nomeArquivo))
            {
                Directory.CreateDirectory(pathAbsoluto);
                File.Create(nomeArquivo).Dispose();
            }

            using (StreamWriter streamWriter = new StreamWriter(nomeArquivo, true))
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }

        }
    }
}
