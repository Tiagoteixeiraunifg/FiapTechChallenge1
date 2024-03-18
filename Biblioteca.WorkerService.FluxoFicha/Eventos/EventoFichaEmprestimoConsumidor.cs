using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Infraestrutura.Logs.Fabricas;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos.Mensageria;
using Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using Biblioteca.Servicos.Notificacoes.Emails.ServicoImplementado;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.WorkerService.FluxoFicha.Eventos
{
    public class EventoFichaEmprestimoConsumidor :  IConsumer<ModeloEnvioFichaEmprestimoOperacores>
    {
        #region PROPRIEDADES

        private ILogger<EventoFichaEmprestimoConsumidor> _logger;

        private ApplicationDbContext Contexto
        {
            get
            {
                return ApplicationDbContext.Instancia();
            }
        }

        private const string UrlServiceBus = "sb://bibliotecaservicebus.servicebus.windows.net/";

        private string NomeFilaError
        {
            get
            {
                return Environment.GetEnvironmentVariable("SERVICE_BUS_NOME_FILA_ERROS") ?? "FILA_ERROR_FICHA";
            }
        }

        private ConsumeContext<ModeloEnvioFichaEmprestimoOperacores> _context;

        #endregion


        public EventoFichaEmprestimoConsumidor()
        {
            _logger = new LogCustomizadoGenerico<EventoFichaEmprestimoConsumidor>("LogConsumidor", new FabricaDeLogs());
        }

        public Task Consume(ConsumeContext<ModeloEnvioFichaEmprestimoOperacores> context)
        {

            try
            {
                _context = context;
                Console.WriteLine($"Info: Worker Capturou a Mensagem. Código: " + context.MessageId);
                var ficha = context.Message;
                ExecuteAhOperacao(ficha);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao executar o comando. StackTrace: {ex.StackTrace ?? ""}");
                Console.WriteLine($"Info: Worker Capturou a Mensagem com ERRO." + context.MessageId);
            }
           
            return context.ConsumeCompleted;
        }

        private void ExecuteAhOperacao(ModeloEnvioFichaEmprestimoOperacores ficha) 
        {
            _logger.LogInformation("Worker: Descobrindo o tipo da operação de FichaEmperstimo");

            switch (ficha.Operacao) 
            {
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CADASTRAR:
                    _logger.LogInformation("Worker: Descoberto o tipo da operação de FichaEmperstimo -> 'CADASTRAR' ");
                    ExecuteCadastroFicha(ficha.Ficha);
                    _context.NotifyConsumed(new TimeSpan(1000), $"{_context.Message.Operacao.ToString()}");
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.EXCLUIR:
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.FINALIZAR:
                    _logger.LogInformation("Worker: Descoberto o tipo da operação de FichaEmperstimo -> 'FINALIZAR' ");
                    ExecuteFinalizacaoFicha(ficha.Ficha);
                    _context.NotifyConsumed(new TimeSpan(1000), $"{_context.Message.Operacao.ToString()}");
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CANCELAR:
                    break;
                default:
                    break;
            }
        }

        private void ExecuteFinalizacaoFicha(string ficha) 
        {
            _logger.LogInformation("Worker: Iniciando a Finalização da Ficha");

            try
            {
                var _fichaAhFinalizar = JsonConvert.DeserializeObject<FichaEmprestimoAluno>(ficha);
                _logger.LogInformation("Worker: Objeto deserializado com sucesso");
                _logger.LogInformation("Worker: Iniciando o a chamada do serviço de finalização da ficha.");

                var log = new LogCustomizadoGenerico<ServicoFichaEmprestimoAlunoImpl>("Servico de Ficha", new FabricaDeLogs());
                using (INotificacaoEmail _notificacao = new NotificacaoEmailImpl())
                using (IServicoFichaEmprestimoAluno _sevicoFicha = new ServicoFichaEmprestimoAlunoImpl(Contexto, log, _notificacao))
                {
                    var resposta = _sevicoFicha.FinalizeFicha(_fichaAhFinalizar);

                    if (resposta.EhValido())
                    {
                        _logger.LogInformation("Serviço Notificação: Notificando o aluno da Finalização da Ficha");
                        _notificacao.NotifiqueFinalizacaoFicha(resposta.ObtenhaRetornoServico());
                        _logger.LogInformation("Serviço Notificação: Aluno Notificado sobre Finalização da Ficha");
                    }
                }

                _logger.LogInformation("Servico de Ficha: Finalizado o Serviço de Finalização da Ficha");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Erro ao tentar executar a Finalização da Ficha");

                

                try
                {
                    _context.Send<ModeloEnvioFichaEmprestimoOperacores>(new Uri(UrlServiceBus+NomeFilaError),
                    new ModeloEnvioFichaEmprestimoOperacores()
                    {
                        Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.ERROR,
                        Data = DateTime.Now,
                        Operador = _context.Message.Operador,
                        Ficha = _context.Message.Ficha
                    },
                    a =>
                    {
                        a.Durable = true;
                    });

                    _context.NotifyFaulted(TimeSpan.FromMinutes(1), "Erro apresentado nessa mensagem", ex);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        private void ExecuteCadastroFicha(string ficha)
        {
            _logger.LogInformation("Worker: Iniciando o Cadastro da Ficha");

            try
            {
                var _fichaAhCadastrar = JsonConvert.DeserializeObject<FichaEmprestimoAluno>(ficha);
                _logger.LogInformation("Worker: Objeto deserializado com sucesso");
                _logger.LogInformation("Worker: Iniciando o a chamada do serviço de Cadastro da ficha.");

                var log = new LogCustomizadoGenerico<ServicoFichaEmprestimoAlunoImpl>("Servico de Ficha", new FabricaDeLogs());
                using (INotificacaoEmail _notificacao = new NotificacaoEmailImpl())
                using (IServicoFichaEmprestimoAluno _sevicoFicha = new ServicoFichaEmprestimoAlunoImpl(Contexto, log, _notificacao))
                {
                    var resposta = _sevicoFicha.CadastreFicha(_fichaAhCadastrar?.ObtenhaDto());

                    if (resposta.EhValido())
                    {
                        _logger.LogInformation("Serviço Notificação: Notificando o aluno do cadastro da Ficha");
                        _notificacao.NotifiqueFinalizacaoFicha(resposta.ObtenhaRetornoServico());
                        _logger.LogInformation($"Serviço Notificação: Aluno notificado sobre cadastro da Ficha: {resposta.ObtenhaRetornoServico().Codigo}");
                    }
                }

                _logger.LogInformation("Worker: Finalizado o Serviço de Cadastro da Ficha");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker: Erro ao cadastrar a Ficha");

                try
                {
                    

                        _context.Send<ModeloEnvioFichaEmprestimoOperacores>(new Uri(UrlServiceBus + NomeFilaError), 
                        new ModeloEnvioFichaEmprestimoOperacores() 
                        {
                            Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.ERROR,
                            Data = DateTime.Now,
                            Operador = _context.Message.Operador,
                            Ficha = _context.Message.Ficha
                        }, 
                        a => 
                        {
                            a.Durable = true;
                        });

                    _context.NotifyFaulted(TimeSpan.FromMinutes(1), "Erro apresentado nessa mensagem", ex);
                }
                catch (Exception )
                {

                    throw;
                }

            }
        }

    }
}
