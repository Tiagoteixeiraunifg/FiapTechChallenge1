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

        private  ILogger<EventoFichaEmprestimoConsumidor> _logger;


        private ApplicationDbContext Contexto
        {
            get
            {
                return ApplicationDbContext.Instancia();
            }
        }

        private string NomeFilaError 
        {
            get 
            {
                return Environment.GetEnvironmentVariable("SERVICE_BUS_NOME_FILA_ERROS") ?? "FILA_ERROR_FICHA";
            }
        }

        private ConsumeContext<ModeloEnvioFichaEmprestimoOperacores> _context;
        public EventoFichaEmprestimoConsumidor()
        {
            _logger = new LogCustomizadoGenerico<EventoFichaEmprestimoConsumidor>("LogConsumidor", new FabricaDeLogs());
        }

        public Task Consume(ConsumeContext<ModeloEnvioFichaEmprestimoOperacores> context)
        {

            try
            {
                _context = context;
                var ficha = context.Message;
                ExecuteAhOperacao(ficha);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao executar o comando. StackTrace: {ex.StackTrace ?? ""}");
                
            }
           
            return context.ConsumeCompleted;
        }


        private void ExecuteAhOperacao(ModeloEnvioFichaEmprestimoOperacores ficha) 
        {
            _logger.LogInformation("Descobrindo o tipo da operação de FichaEmperstimo");

            switch (ficha.Operacao) 
            {
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CADASTRAR:
                    _logger.LogInformation("Descoberto o tipo da operação de FichaEmperstimo -> 'CADASTRAR' ");
                    ExecuteCadastroFicha(ficha.Ficha);
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.EXCLUIR:
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.FINALIZAR:
                    _logger.LogInformation("Descoberto o tipo da operação de FichaEmperstimo -> 'FINALIZAR' ");
                    ExecuteFinalizacaoFicha(ficha.Ficha);
                    break;
                case Negocio.Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CANCELAR:
                    break;
                default:
                    break;
            }
        }


        private void ExecuteFinalizacaoFicha(string ficha) 
        {
            _logger.LogInformation("Iniciando a Finalização da Ficha");

            try
            {
                var _fichaAhFinalizar = JsonConvert.DeserializeObject<FichaEmprestimoAluno>(ficha);
                _logger.LogInformation("Objeto deserializado com sucesso");
                _logger.LogInformation("Iniciando o a chamada do serviço de finalização da ficha.");

                var log = new LogCustomizadoGenerico<ServicoFichaEmprestimoAlunoImpl>("Servico de Ficha", new FabricaDeLogs());
                using (INotificacaoEmail _notificacao = new NotificacaoEmailImpl())
                using (IServicoFichaEmprestimoAluno _sevicoFicha = new ServicoFichaEmprestimoAlunoImpl(Contexto, log, _notificacao))
                {
                    var resposta = _sevicoFicha.FinalizeFicha(_fichaAhFinalizar);

                    if (resposta.EhValido())
                    {
                        _logger.LogInformation("Notificando o aluno da Finalização da Ficha");
                        _notificacao.NotifiqueFinalizacaoFicha(resposta.ObtenhaRetornoServico());
                    }
                }

                _logger.LogInformation("Finalizado o Serviço de Finalização da Ficha");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Iniciando a Finalização da Ficha");
                try
                {
                    _context.Send<ModeloEnvioFichaEmprestimoOperacores>(new Uri(NomeFilaError),
                    new ModeloEnvioFichaEmprestimoOperacores()
                    {
                        Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CANCELAR,
                        Data = DateTime.Now,
                        Operador = _context.Message.Operador,
                        Ficha = _context.Message.Ficha
                    },
                    a =>
                    {
                        a.Durable = true;
                    });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        private void ExecuteCadastroFicha(string ficha)
        {
            _logger.LogInformation("Iniciando o Cadastro da Ficha");

            try
            {
                var _fichaAhCadastrar = JsonConvert.DeserializeObject<FichaEmprestimoAluno>(ficha);
                _logger.LogInformation("Objeto deserializado com sucesso");
                _logger.LogInformation("Iniciando o a chamada do serviço de Cadastro da ficha.");

                var log = new LogCustomizadoGenerico<ServicoFichaEmprestimoAlunoImpl>("Servico de Ficha", new FabricaDeLogs());
                using (INotificacaoEmail _notificacao = new NotificacaoEmailImpl())
                using (IServicoFichaEmprestimoAluno _sevicoFicha = new ServicoFichaEmprestimoAlunoImpl(Contexto, log, _notificacao))
                {
                    var resposta = _sevicoFicha.CadastreFicha(_fichaAhCadastrar?.ObtenhaDto());

                    if (resposta.EhValido())
                    {
                        _logger.LogInformation("Notificando o aluno da Finalização da Ficha");
                        _notificacao.NotifiqueFinalizacaoFicha(resposta.ObtenhaRetornoServico());
                    }
                }

                _logger.LogInformation("Finalizado o Serviço de Cadastro da Ficha");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar a Ficha");

                try
                {
                        _context.Send<ModeloEnvioFichaEmprestimoOperacores>(new Uri(NomeFilaError), 
                        new ModeloEnvioFichaEmprestimoOperacores() 
                        {
                            Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CANCELAR,
                            Data = DateTime.Now,
                            Operador = _context.Message.Operador,
                            Ficha = _context.Message.Ficha
                        }, 
                        a => 
                        {
                            a.Durable = true;
                        });
                }
                catch (Exception )
                {

                    throw;
                }

            }
        }

    }
}
