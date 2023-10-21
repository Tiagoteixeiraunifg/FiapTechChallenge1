using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Validacoes.EmprestimoAlunos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoFichaEmprestimoAlunoImpl : EFRepositorioGenerico<FichaEmprestimoAluno>, IServicoFichaEmprestimoAluno
    {

        
        private readonly ILogger _logger;

        public ServicoFichaEmprestimoAlunoImpl(ApplicationDbContext contexto, ILogger<ServicoFichaEmprestimoAlunoImpl> logger) : base(contexto)
        {
            this._logger = logger;
            this._contexto = contexto;
        }

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

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Executando validação total nos dados para cadastro.");

            if (!dados.ValideCadastroFicha()) 
            {
                return dados.RetornarInconsistencias();
            }

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Fim da validação total nos dados para cadastro.");

            _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Iniciando persistência dos dados.");

            try
            {
                base.Cadastre(fichaNova);
                var cadastroAtualizado = _DbSet.AsNoTracking().Where(x => x.Codigo == fichaNova.Codigo).FirstOrDefault();
                _logger.LogInformation("Serviço 'Serviço de Ficha Emprestimo': Fim da persistência dos dados.");
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = cadastroAtualizado, Mensagem = "Cadastrado com Sucesso" };

            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Ficha Emprestimo': Erro nos dados para cadastro.", ex);
                return new InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno>() { _RetornoServico = ex, Mensagem = "Erro no cadastro do Livro" };
            }

        }



        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ExcluaFicha(int FihaId)
        {
            throw new NotImplementedException();
        }

        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> FinalizeFicha(FichaEmprestimoAlunoDto dados)
        {
            throw new NotImplementedException();
        }

        public FichaEmprestimoAluno ObtenhaFichaPorCodigo(int FichaId)
        {
            throw new NotImplementedException();
        }

        public IList<FichaEmprestimoAluno> ObtenhaFichasDoAlunoPorCodigo(int AlunoId, int limiteRegistros)
        {
            throw new NotImplementedException();
        }

        public IList<FichaEmprestimoAluno> ObtenhaFichasDoAlunoPorCodigoEhIntervaloEhSituacao(int AlunoId, DateTime DataInicial, DateTime DataFinal, FichaEmprestimoAlunoStatusEnum situacao, int limiteRegistros)
        {
            throw new NotImplementedException();
        }

        public IList<FichaEmprestimoAluno> ObtenhaFichasEmAtrasoDeEntregaPorIntervalo(DateTime DataInicial, DateTime DataFinal, int limiteRegistros)
        {
            throw new NotImplementedException();
        }

        public IList<FichaEmprestimoAluno> ObtenhaTodasFichas()
        {
            throw new NotImplementedException();
        }
    }
}
