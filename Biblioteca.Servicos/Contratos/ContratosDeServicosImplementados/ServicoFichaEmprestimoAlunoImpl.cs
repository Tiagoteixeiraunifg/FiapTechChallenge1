using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Validacoes.EmprestimoAlunos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    /// <summary>
    /// Prover os serviços para emprestimos de livros da biblioteca.
    /// </summary>
    public class ServicoFichaEmprestimoAlunoImpl : EFRepositorioGenerico<FichaEmprestimoAluno>, IServicoFichaEmprestimoAluno
    {

        
        private readonly ILogger _logger;

        public ServicoFichaEmprestimoAlunoImpl(ApplicationDbContext contexto, ILogger<ServicoFichaEmprestimoAlunoImpl> logger) : base(contexto)
        {
            this._logger = logger;
            this._contexto = contexto;
        }

        /// <summary>
        /// Cadastro de Nova Ficha de Emprestimo
        /// </summary>
        /// <param name="dados">Os Dados para Validação e Cadastro da Ficha</param>
        /// <returns>InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno></returns>
        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> CadastreFicha(FichaEmprestimoAlunoDto dados)
        {
            var fichaNova = new FichaEmprestimoAluno();
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Executando ajuste nos dados para cadastro.");
            fichaNova = dados.ObtenhaEntidade();
            fichaNova.DataCriacao = DateTime.Now;
            fichaNova.DataAtualizacao = DateTime.Now;
            fichaNova.DataEmprestimo = DateTime.Now;
            fichaNova.StatusEmprestimo = FichaEmprestimoAlunoStatusEnum.NORMAL;
            fichaNova.Codigo = Guid.NewGuid();

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Executando pré validação nos dados para cadastro.");

            var inconsistencias = new ServicoValidacaoFichaEmprestimoAluno().ValideFichaCadastro(fichaNova);
            if (!inconsistencias.EhValido()) return inconsistencias;

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Fim da pré validação nos dados para cadastro.");

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Iniciando persistência dos dados.");

            try
            {
                base.Cadastre(fichaNova);
                var cadastroAtualizado = _DbSet.AsNoTracking().Where(x => x.Codigo == fichaNova.Codigo).Include(X => X.FichaEmprestimoItens).FirstOrDefault();
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Fim da persistência dos dados.");
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = cadastroAtualizado, Mensagem = "Cadastrado com Sucesso" };

            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro nos dados para cadastro.", ex);
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = ex, Mensagem = "Erro no cadastro da Ficha" };
            }

        }

        /// <summary>
        /// Exclui a Ficha de Emprestimo
        /// </summary>
        /// <param name="FichaId">Os Dados para Validação e Exclusão da Ficha</param>
        /// <returns>InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno></returns>
        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ExcluaFicha(int FichaId)
        {

            InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> retorno = null;

            try
            {
                var ficha = base.ObtenhaPorId(FichaId);

                if (!ficha.PossuiValor())
                {
                    _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Ficha não encontrada");
                    retorno = new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = null, Mensagem = "Ficha não encontrada" };
                    return retorno;
                }

                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Executando validação para exclusão");
                var itensTodosAhEntregar = ficha.FichaEmprestimoItens.Any() && ficha.FichaEmprestimoItens.Any(x => x.StatusItem == FichaEmprestimoAlunoItensStatusEnum.A_ENTREGAR);
                var ficaSemItens = !ficha.FichaEmprestimoItens.Any();
                var fichaAberta = ficha.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.NORMAL;

                if (fichaAberta && itensTodosAhEntregar || ficaSemItens)
                {
                    _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Iniciando a exclusão");
                    base.Delete(FichaId);
                    _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Ficha removida.");
                    retorno = new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { Mensagem = $"Livro de código {FichaId.ToString("0000000")} foi deletado com sucesso" };
                }
                else 
                {
                    retorno = new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = null, Mensagem = "Ficha não pode ser excluída." };
                    return retorno;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro nos dados para cadastro.", ex);
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { Mensagem = $"Erro na exclusão da Ficha: Erro -> {ex.Message}" };
            }
            
        }

        /// <summary>
        /// Executa a Entrega do Livro Individual da Ficha
        /// </summary>
        /// <param name="FichaId">Identificação da Ficha</param>
        /// <param name="LivroId">Identificação do Livro</param>
        /// <returns>InconsistenciaDeValidacaoTipado</returns>
        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ExecuteEntregaDeLivro(int FichaId, int LivroId)
        {
            try
            {
                if (!FichaId.PossuiValor() || !LivroId.PossuiValor()) throw new InvalidOperationException("Não foi informado os Parametros");

                using (IRepositorioGenerico<FichaEmprestimoItem> servico = new EFRepositorioGenerico<FichaEmprestimoItem>(_contexto))
                {
                    var livro = servico.ObtenhaDbSet().AsNoTracking().Where(x => x.LivroId == LivroId && x.FichaEmprestimoAlunoId == FichaId).FirstOrDefault();
                    
                    if (!livro.PossuiValor()) throw new Exception("Objeto não encontrado");

                    livro.DataStatusItem = DateTime.Now;
                    livro.StatusItem = FichaEmprestimoAlunoItensStatusEnum.ENTREGUE;

                    servico.Altere(livro);

                    _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo':Livro entregue.");
                    return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() {TipoValidacao = Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.AVISO, Mensagem = $"Livro de código {livro.LivroId.ToString("0000000")} foi entregue com sucesso" };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro nos dados para Finalização da Ficha.", ex);
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { Mensagem = $"Erro na entrega do livro: Erro -> {ex.Message}" };
            }
  
        }

        /// <summary>
        /// Finaliza a Ficha de Emprestimo do Aluno
        /// </summary>
        /// <param name="dados">Os Dados Para Finalização da Ficha De Emprestimo</param>
        /// <returns>InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno></returns>
        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> FinalizeFicha(FichaEmprestimoAluno dados)
        {
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da Finalização da Ficha");
            var fichaNova = new FichaEmprestimoAluno();
            try
            {
                fichaNova = dados;
                fichaNova.StatusEmprestimo = FichaEmprestimoAlunoStatusEnum.ENTREGUE;
                fichaNova.DataEntregaEmprestimo = DateTime.Now;
                fichaNova.DataAtualizacao = DateTime.Now;

                foreach (var item in fichaNova.FichaEmprestimoItens)
                {
                    if(item.DataStatusItem == DateTime.MinValue || item.StatusItem == FichaEmprestimoAlunoItensStatusEnum.A_ENTREGAR) 
                    {
                        item.DataStatusItem = DateTime.Now;
                        item.StatusItem = FichaEmprestimoAlunoItensStatusEnum.ENTREGUE;
                    }
                }

                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Validandação da Finalização da Ficha");
                var inconsistencias = new ServicoValidacaoFichaEmprestimoAluno().ValideFinalizacaoFicha(fichaNova);
                
                if (!inconsistencias.EhValido()) 
                {
                    _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Encontrado Inconsistencias na Finalização da Ficha");
                    return inconsistencias;
                }

                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da persistência da Finalização da Ficha");
                base.Altere(fichaNova);

                var fichaFinalizada = _DbSet.AsNoTracking().Where(x => x.Codigo == fichaNova.Codigo).FirstOrDefault();
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Fim da persistência dos dados.");
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = fichaFinalizada, Mensagem = "Finalizada com Sucesso" };


            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro nos dados para Finalização da Ficha.", ex);
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() {Mensagem = $"Erro na Finalização da Ficha: Erro -> {ex.Message}" };
            }

        }

        /// <summary>
        /// Obtem a Ficha pelo Código
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns></returns>
        public FichaEmprestimoAluno ObtenhaFichaPorCodigo(int FichaId)
        {
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da busca da Ficha");
       
            if (!FichaId.PossuiValor()) return null;

            try
            {
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Buscando a Ficha");
                return base.ObtenhaDbSet().Where(x => x.Id == FichaId).Include(x => x.FichaEmprestimoItens).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro na busca dos dados da Ficha.", ex);
                return null;
            }
        }

        /// <summary>
        /// Obtem uma coleção de Fichas pelo código do aluno e limite de registros
        /// </summary>
        /// <param name="AlunoId">Identificação do Aluno</param>
        /// <param name="limiteRegistros">Limite de Registros Retornados</param>
        /// <returns>Lista de Ficha de Emprestimo</returns>
        public IList<FichaEmprestimoAluno> ObtenhaFichasDoAlunoPorCodigo(int AlunoId, int limiteRegistros)
        {
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da busca das Fichas");

            if (!AlunoId.PossuiValor() || !limiteRegistros.PossuiValor()) return null;


            try
            {
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Buscando as Fichas");
                return base.ObtenhaDbSet().AsNoTracking().Where(x => x.AlunoId == AlunoId)
                    .Take(limiteRegistros)
                    .Include(x => x.FichaEmprestimoItens)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro na busca dos dados das Fichas.", ex);
                return null;
            }
        }

        /// <summary>
        /// Obtem as fichas do aluno seguindo os critérios
        /// </summary>
        /// <param name="AlunoId">Identificação do Aluno</param>
        /// <param name="DataInicial">Data Inicial de Cadastro</param>
        /// <param name="DataFinal">Data Final de Cadastro</param>
        /// <param name="situacao">Situação da Ficha</param>
        /// <param name="limiteRegistros">Limite de Registros Retornados</param>
        /// <returns>Lista de ficha de Alunos</returns>
        public IList<FichaEmprestimoAluno> ObtenhaFichasDoAlunoPorCodigoEhIntervaloEhSituacao(int AlunoId, 
                                                                                              DateTime DataInicial, 
                                                                                              DateTime DataFinal, 
                                                                                              FichaEmprestimoAlunoStatusEnum situacao, 
                                                                                              int limiteRegistros)
        {
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da busca das Fichas");

            if (!AlunoId.PossuiValor() || !limiteRegistros.PossuiValor() || !DataInicial.PossuiValor() || !DataFinal.PossuiValor() || !situacao.PossuiValor()) return null;


            try
            {
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Buscando as Fichas");
                return base.ObtenhaDbSet().AsNoTracking().Where(x => x.AlunoId == AlunoId 
                                                                    && x.StatusEmprestimo == situacao 
                                                                    && x.DataCriacao.Date >= DataInicial.Date 
                                                                    && x.DataCriacao.Date <= DataFinal.Date)
                                                                        .Take(limiteRegistros)
                                                                        .Include(x => x.FichaEmprestimoItens)
                                                                        .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro na busca dos dados das Fichas.", ex);
                return null;
            }
        }

        /// <summary>
        /// Obtem as fichas atrasadas de entrega em 8 dias corridos
        /// </summary>
        /// <param name="DataInicial">Data Inicial de Cadastro</param>
        /// <param name="DataFinal">Data final de cadastro</param>
        /// <param name="limiteRegistros">LImite de registros retornados</param>
        /// <returns>Lista de Fichas em Atraso</returns>
        public IList<FichaEmprestimoAluno> ObtenhaFichasEmAtrasoDeEntregaPorIntervalo(DateTime DataInicial, 
                                                                                      DateTime DataFinal, 
                                                                                      int limiteRegistros)
        {
            //considerando que 8 dias seja um atraso na entrega do livro.
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da busca das Fichas");
            try
            {
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Buscando as Fichas");
                var fichasEncontradas = base.ObtenhaDbSet().AsNoTracking().Where(x => x.DataCriacao.Date >= DataInicial.Date
                                                                    && x.DataCriacao.Date <= DataFinal.Date)
                                                                        .Take(limiteRegistros)
                                                                        .Include(x => x.FichaEmprestimoItens)
                                                                        .ToList();
                
                var FichasAtrasadasNoIntervalo = new List<FichaEmprestimoAluno>();
                
                foreach (var Ficha in fichasEncontradas)
                {
                    var atrasada = (Ficha.DataCriacao.Date - DateTime.Now.Date).Days > 8;
                    FichasAtrasadasNoIntervalo.Add(Ficha);
                }

                return FichasAtrasadasNoIntervalo;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro na busca dos dados das Fichas.", ex);
                return null;

            }
            
        }

        /// <summary>
        /// Obtem uma coleção total das Fichas de Cadastro
        /// </summary>
        /// <returns></returns>
        public IList<FichaEmprestimoAluno> ObtenhaTodasFichas()
        {
            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Inicio da busca das Fichas");
            try
            {
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Buscando as Fichas");
                return base.ObtenhaDbSet().AsNoTracking()
                    .Include(x => x.FichaEmprestimoItens)
                    .ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro na busca dos dados das Fichas.", ex);
                return null;

            }
        }
    }
}
