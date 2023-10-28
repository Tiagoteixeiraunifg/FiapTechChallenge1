using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using Biblioteca.Servicos.Notificacoes.Emails.ServicoImplementado;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Servicos.Notificacoes.Emails
{
    public static class InicializacaoServicoNotificacao
    {
        private static INotificacaoEmail _NotificacaoEmail;
      

        public static IServiceCollection InicieServicoNotificacao(this IServiceCollection services, IConfiguration configuracao) 
        {

            _NotificacaoEmail = new NotificacaoEmailImpl();
            _NotificacaoEmail.InicializarServico(configuracao);

            services.AddSingleton<INotificacaoEmail, NotificacaoEmailImpl>();

            return services;
        }
    }
}
