using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.WorkerService.FluxoFicha.Eventos
{
    public abstract class EventoFichaConsumidorAbstrato
    {
        private readonly ILogger _logger;
        private readonly IServicoFichaEmprestimoAluno _servicoDeFicha;
        private readonly INotificacaoEmail _notificacao;

        protected EventoFichaConsumidorAbstrato(ILogger logger, IServicoFichaEmprestimoAluno servicoDeFicha, INotificacaoEmail notificacao)
        {
            _logger = logger;
            _servicoDeFicha = servicoDeFicha;
            _notificacao = notificacao;
        }




    }
}
