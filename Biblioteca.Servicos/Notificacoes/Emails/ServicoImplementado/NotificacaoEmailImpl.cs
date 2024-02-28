using Biblioteca.Infraestrutura.Logs.Fabricas;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Utilidades.Emails;
using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Microsoft.Extensions.Logging;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Contextos;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Negocio.Entidades.Livros;


namespace Biblioteca.Servicos.Notificacoes.Emails.ServicoImplementado
{
    public class NotificacaoEmailImpl : INotificacaoEmail
    {
        #region PROPRIEDADES
        private ILogger<NotificacaoEmailImpl> _Logger;
        private IConfiguration _config;

        private string NOTIFICACOES_HABILITADAS;
        private string REMETENTE;
        private string SSL;
        private string PORTA;
        private string PASSWORD;
        private string HOST;
        private const string HTML = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset='utf-8'>\r\n    <meta http-equiv='X-UA-Compatible' content='IE=edge'>\r\n    <title>#TIPO_EMAIL#</title>\r\n    <meta name='viewport' content='width=device-width, initial-scale=1'>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"form-group\">\r\n            <div class=\"row\">\r\n                <p>#MENSAGEM#</p>  \r\n            </div>          \r\n        </div>\r\n    </div>    \r\n</body>\r\n</html>";

        #endregion

        #region CONSTRUTORES
        public NotificacaoEmailImpl()
        {
            NOTIFICACOES_HABILITADAS = "NAO";
        }

        public NotificacaoEmailImpl(IConfiguration config)
        {
            _config = config;
            PreenchaAsConfiguracoes();
        }
        #endregion

        #region METODOS PUBLICOS
        public bool NotifiqueFichaAtraso8Dias(FichaEmprestimoAluno ficha)
        {
            if (!ficha.PossuiValor() || NOTIFICACOES_HABILITADAS.Equals("NAO")) return false;

            var emailAluno = ObtenhaNomeEhEmailDoAluno(ficha.AlunoId).Split("/");
            var listaDeLivros = ObtenhaDescricaoDosLivros(ficha.FichaEmprestimoItens.ToList());



            var mensagem = new MensagemEmail()
            {
                AssuntoEmail = "Aviso de Ficha de Emprestimo em Atraso de Entrega",
                TipoDeMensagem = Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.AVISO,
                TipoEmail = Negocio.Enumeradores.Emails.TipoEmailEnum.SEM_ANEXOS,
                TextoMensagemEmail = ObtenhaTextoCorpoEmail(emailAluno[0], listaDeLivros, TipoOperacaoNotificacaoEnum.FINALIZACAO),
                DestinatariosEmail = new DestinatariosEmail()
                {
                    Destinatarios = new List<MailAddress>() { new MailAddress(emailAluno[1]) }
                }
            };

            return CrieEhEnvieEmail(mensagem);
        }

        public bool NotifiqueFinalizacaoFicha(FichaEmprestimoAluno ficha)
        {
            if (!ficha.PossuiValor() || NOTIFICACOES_HABILITADAS.Equals("NAO")) return false;

            var emailAluno = ObtenhaNomeEhEmailDoAluno(ficha.AlunoId).Split("/");
            var listaDeLivros = ObtenhaDescricaoDosLivros(ficha.FichaEmprestimoItens.ToList());



            var mensagem = new MensagemEmail()
            {
                AssuntoEmail = "Gravação de Finalização Ficha de Emprestimo",
                TipoDeMensagem = Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.AVISO,
                TipoEmail = Negocio.Enumeradores.Emails.TipoEmailEnum.SEM_ANEXOS,
                TextoMensagemEmail = ObtenhaTextoCorpoEmail(emailAluno[0], listaDeLivros, TipoOperacaoNotificacaoEnum.FINALIZACAO),
                DestinatariosEmail = new DestinatariosEmail()
                {
                    Destinatarios = new List<MailAddress>() { new MailAddress(emailAluno[1]) }
                }
            };

            return CrieEhEnvieEmail(mensagem);
        }

        public bool NotifiqueGravacaoFicha(FichaEmprestimoAluno ficha)
        {
            if (!ficha.PossuiValor() || NOTIFICACOES_HABILITADAS.Equals("NAO")) return false;


            var emailAluno = ObtenhaNomeEhEmailDoAluno(ficha.AlunoId).Split("/");
            var listaDeLivros = ObtenhaDescricaoDosLivros(ficha.FichaEmprestimoItens.ToList());



            var mensagem = new MensagemEmail()
            {
                AssuntoEmail = "Gravação de Ficha de Emprestimo",
                TipoDeMensagem = Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.AVISO,
                TipoEmail = Negocio.Enumeradores.Emails.TipoEmailEnum.SEM_ANEXOS,
                TextoMensagemEmail = ObtenhaTextoCorpoEmail(emailAluno[0], listaDeLivros, TipoOperacaoNotificacaoEnum.GRAVACAO),
                DestinatariosEmail = new DestinatariosEmail()
                {
                    Destinatarios = new List<MailAddress>() { new MailAddress(emailAluno[1]) }
                }
            };

            return CrieEhEnvieEmail(mensagem);

        }
        #endregion

        #region METODOS PRIVADOS

        private void PreenchaAsConfiguracoes()
        {

            if (!_config.PossuiValor()) { NOTIFICACOES_HABILITADAS = "NAO"; return; };
            NOTIFICACOES_HABILITADAS = _config.GetSection("HabilitaNotificacoes").Value;
            REMETENTE = _config.GetSection("Remetente").Value;
            SSL = _config.GetSection("Ssl").Value;
            PORTA = _config.GetSection("Porta").Value;
            PASSWORD = _config.GetSection("PwdEmail").Value;
            HOST = _config.GetSection("Host").Value;

        }

        private bool CrieEhEnvieEmail(MensagemEmail mensagemEmail)
        {
            bool enviado = false;

            try
            {

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = mensagemEmail.AssuntoEmail;
                mailMessage.From = new MailAddress(REMETENTE);

                //lista de destinatários
                foreach (MailAddress item in mensagemEmail.DestinatariosEmail.Destinatarios)
                {
                    mailMessage.To.Add(item);
                }
                //lista de destinatarios com cópia
                if (mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.AVISO_CC
                    || mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.COMUN_CC
                    || mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.RELATORIO_CC)
                {
                    foreach (MailAddress item in mensagemEmail.DestinatariosEmail.Destinatarios_cc)
                    {
                        mailMessage.CC.Add(item);
                    }
                }
                //lista de destinatários com cópia oculta
                if (mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.AVISO_CCO
                    || mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.COMUN_CCO
                    || mensagemEmail.TipoDeMensagem == Negocio.Enumeradores.Emails.TipoMensagemEmailEnum.RELATORIO_CCO)
                {
                    foreach (MailAddress item in mensagemEmail.DestinatariosEmail.Destinatarios_cco)
                    {
                        mailMessage.Bcc.Add(item);
                    }
                }

                var corpoDoEmail = HTML.Replace("#TIPO_EMAIL#", mensagemEmail.TipoEmail.ToString()).Replace("#MENSAGEM#", mensagemEmail.TextoMensagemEmail);
                mailMessage.Priority = MailPriority.High;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.Body = corpoDoEmail;

                if (mensagemEmail.AnexosEmail.PossuiValor()
                    && mensagemEmail.AnexosEmail.Anexos.PossuiValor()
                    && mensagemEmail.AnexosEmail.Anexos.PossuiLinhas())
                {
                    foreach (var item in mensagemEmail.AnexosEmail.Anexos)
                    {
                        Attachment anexado = new Attachment(item, MediaTypeNames.Application.Octet);
                        mailMessage.Attachments.Add(anexado);
                    }
                }

                //configurando o cliente
                SmtpClient cliente = new SmtpClient(HOST, int.Parse(PORTA));

                if (SSL.Equals("SIM"))
                {
                    cliente.EnableSsl = true;

                }
                else if (SSL.Equals("NAO"))
                {
                    cliente.EnableSsl = false;
                }

                // Usa as credenciais padroes??
                cliente.UseDefaultCredentials = false;
                cliente.Credentials = new NetworkCredential(REMETENTE, PASSWORD);

                cliente.Send(mailMessage);


            }
            catch (Exception ex)
            {
                _Logger.LogError($"Erro ao enviar email, erro:{ex.Message} ", ex);
                return false;

            }

            return enviado;
        }

        private string ObtenhaNomeEhEmailDoAluno(int IdAluno)
        {
            string nomeeemail = "";

            try
            {
                using (IRepositorioGenerico<Aluno> Servico = new EFRepositorioGenerico<Aluno>(ApplicationDbContext.NovaInstancia()))
                {
                    var email = Servico.ObtenhaDbSet().AsNoTracking().Where(x => x.Id == IdAluno).FirstOrDefault().Email.ToLowerInvariant();
                    var nome = Servico.ObtenhaDbSet().AsNoTracking().Where(x => x.Id == IdAluno).FirstOrDefault().Nome.ToLowerInvariant();
                    if (email.PossuiValor() && nome.PossuiValor())
                    {
                        nomeeemail = $"{nome}/{email}";
                    }
                    return nomeeemail;
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Erro ao obter email do aluno. Erro: {ex.Message}", ex);
                return null;
            }

        }

        private string ObtenhaTextoCorpoEmail(string Aluno, List<string> livros, TipoOperacaoNotificacaoEnum tipoMensagem)
        {
            string mensagemContatenada = "";

            switch (tipoMensagem)
            {
                case TipoOperacaoNotificacaoEnum.GRAVACAO:
                    mensagemContatenada = $"Mensagem automática sistema: Informamos que o aluno: {Aluno.ToUpper()}, iniciou um empréstimo com os seguintes exemplares: ";
                    break;
                case TipoOperacaoNotificacaoEnum.FINALIZACAO:
                    mensagemContatenada = $"Mensagem automática sistema: Informamos que o aluno: {Aluno.ToUpper()}, finalizou o empréstimo com os seguintes exemplares: ";
                    break;
                case TipoOperacaoNotificacaoEnum.AVISO:
                    mensagemContatenada = $"Mensagem automática sistema: Informamos que o aluno: {Aluno.ToUpper()}, está com o empréstimo em atraso dos seguintes exemplares: ";
                    break;
                default:
                    break;
            }


            int totalDeLivros = livros.Count;
            for (int i = 0; i < livros.Count; i++)
            {
                if (i != totalDeLivros)
                {
                    if (totalDeLivros == 1)
                    {
                        mensagemContatenada += $" {livros[i].ToUpper()}.";
                    }
                    else
                    {
                        mensagemContatenada += $" {livros[i].ToUpper()},";
                    }

                }
                else
                {
                    mensagemContatenada += $" {livros[i].ToUpper()}.";
                }
            }

            return mensagemContatenada;
        }

        private List<string> ObtenhaDescricaoDosLivros(List<FichaEmprestimoItem> lista)
        {
            var listaDeDescricoes = new List<string>();

            try
            {
                using (IRepositorioGenerico<Livro> servico = new EFRepositorioGenerico<Livro>(ApplicationDbContext.NovaInstancia()))
                {
                    foreach (var Livro in lista)
                    {
                        listaDeDescricoes.Add(servico.ObtenhaDbSet().AsNoTracking().Where(x => x.Id == Livro.LivroId).FirstOrDefault().Titulo);
                    }
                }

                return listaDeDescricoes;
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Erro ao obter as descrições dos livros. Erro: {ex.Message}", ex);
                return listaDeDescricoes;

            }

        }

        public void InicializarServico(IConfiguration configuracao)
        {
            this._config = configuracao;
            PreenchaAsConfiguracoes();
        }

        public void Dispose()
        {
            
        }


        #endregion

    }
}
