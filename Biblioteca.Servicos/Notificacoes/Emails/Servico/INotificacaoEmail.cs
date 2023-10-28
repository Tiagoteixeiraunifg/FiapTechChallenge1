using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Notificacoes.Emails.Servico
{
    public interface INotificacaoEmail
    {
        bool NotifiqueGravacaoFicha(FichaEmprestimoAluno ficha);

        bool NotifiqueFichaAtraso8Dias(FichaEmprestimoAluno ficha);

        bool NotifiqueFinalizacaoFicha(FichaEmprestimoAluno ficha);

        void InicializarServico(IConfiguration configuracao);

    }
}
