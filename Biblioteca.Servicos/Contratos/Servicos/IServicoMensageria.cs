using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoMensageria : IDisposable
    {
        public  Task<ISendEndpoint> Instancia();

        public Task<ISendEndpoint> InstanciaError();
    }
}
