using Biblioteca.Servicos.Contratos.Servicos;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoMensageria : IServicoMensageria
    {

        private readonly IBus _bus;
        private string _NOME_FILA;
        private string _NOME_FILA_ERROR;
        private IConfiguration _configuracao;

        public ServicoMensageria(IBus bus, IConfiguration configuracao)
        {
            _bus = bus;
            _configuracao = configuracao;
            _NOME_FILA = _configuracao.GetSection("MassTransitAzure")["Fila"] ?? Environment.GetEnvironmentVariable("SERVICE_BUS_NOME_FILA");
            _NOME_FILA_ERROR = _configuracao.GetSection("MassTransitAzure")["FilaError"] ?? Environment.GetEnvironmentVariable("SERVICE_BUS_NOME_FILA_ERROS");
        }

        public void Dispose()
        {
            
        }

        public async Task<ISendEndpoint> Instancia()
        {
            var PontoDeEnvio = await _bus.GetSendEndpoint(new Uri($"queue:{_NOME_FILA}"));
            return PontoDeEnvio;
        }

        public async Task<ISendEndpoint> InstanciaError() 
        {
            var PontoDeEnvio = await _bus.GetSendEndpoint(new Uri($"queue:{_NOME_FILA_ERROR}"));
            return PontoDeEnvio;
        }
    }
}
