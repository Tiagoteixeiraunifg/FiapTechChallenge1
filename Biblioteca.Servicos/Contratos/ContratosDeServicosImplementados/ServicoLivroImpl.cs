using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.Livros;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Validacoes.Livros;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoLivroImpl : EFRepositorioGenerico<Livro>, ILivroRepositorio, IServicoLivro
    {

        private ILogger<ServicoLivroImpl> _logger;
        public ServicoLivroImpl(ApplicationDbContext contexto, ILogger<ServicoLivroImpl> logger) : base(contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        public InconsistenciaDeValidacaoTipado<Livro> Atualizar(LivroDto livro)
        {
            Livro livroAtualizado = null;
            
           
            _logger.LogInformation("Serviço 'Serviço de Livro': Executando ajuste nos dados para atualização.");
            
            var entidadeAtual = base._DbSet.AsNoTracking().Where(x => x.Id == livro.Id).FirstOrDefault();
            var entidade = livro.ObtenhaEntidade();
            entidade.DataAtualizacao = DateTime.Now;
            entidade.Codigo = entidadeAtual.Codigo;
            
            _logger.LogInformation("Serviço 'Serviço de Livro': Início da Validação do Livro.");
            if (!livro.IsValid()) 
            {
                _logger.LogInformation("Serviço 'Serviço de Livro': Encontrado Inconsistências.");
                return livro.RetornarInconsistencia(); 
            }

            _logger.LogInformation("Serviço 'Serviço de Livro': Executando a atualização.");

            try
            {
                
                _logger.LogInformation("Serviço 'Serviço de Livro': Executando a atualização no banco de dados.");
                base.Altere(entidade);


                livroAtualizado = base._DbSet.AsNoTracking().Where(x => x.Codigo == livro.Codigo).FirstOrDefault();

                _logger.LogInformation("Serviço 'Serviço de Livro': Devolvendo o Resultado.");
                return new InconsistenciaDeValidacaoTipado<Livro>() { _RetornoServico = livroAtualizado, Mensagem = "Atualizado com Sucesso" };
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Livro': Erro ao atualizar o Livro.", ex);
                return new InconsistenciaDeValidacaoTipado<Livro>() { _RetornoServico = ex, Mensagem = "Erro na atualização do Livro" };
            }
            
           
        }

        public InconsistenciaDeValidacaoTipado<Livro> Cadastrar(LivroDto livro)
        {
            var livroModel = livro.ObtenhaEntidade();
            
            _logger.LogInformation("Serviço 'Serviço de Livro': Preparando os dados para o cadsatro.");
           
            livroModel.Codigo = Guid.NewGuid();
            livroModel.DataCriacao = DateTime.Now;
            livroModel.DataAtualizacao = DateTime.Now;

            _logger.LogInformation("Serviço 'Serviço de Livro': Início da Validação do Livro");

            var validacoesIniciais = new ServicoLivroValidador().ValideInicial(livroModel);
            if (!validacoesIniciais.EhValido()) return validacoesIniciais;


            if (!livro.IsValid()) 
            {
                _logger.LogInformation("Serviço 'Serviço de Livro': Encontrado inconsistências.");
                return livro.RetornarInconsistencia(); 
            }
            
            try
            {
              
                _logger.LogInformation("Serviço 'Serviço de Livro': Executando o cadsatro.");
                base.Cadastre(livroModel);
                var cadastroAtualizado = base._DbSet.AsNoTracking().Where(x => x.Codigo == livroModel.Codigo).Include(x => x.Autores).Include(x => x.Editora).FirstOrDefault();

                _logger.LogInformation("Serviço 'Serviço de Livro': Devolvendo o resultado.");
                return new InconsistenciaDeValidacaoTipado<Livro>() { _RetornoServico = cadastroAtualizado, Mensagem = "Cadastrado com Sucesso" };
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Livro': Erro ao cadastrar o Livro.", ex);
                return new InconsistenciaDeValidacaoTipado<Livro>() { _RetornoServico = ex, Mensagem = "Erro no cadastro do Livro" };
            }
           
        }

        public void CadastrarLivros(IList<Livro> livros)
        {
            CadastrarLivros(livros);
        }
                
        public Livro ConsultarLivroPorIdEditar(int idEditora)
        {
            throw new NotImplementedException();
        }

        public InconsistenciaDeValidacaoTipado<Livro> Deletar(int Id)
        {
            bool livroEmUso = false;

            _logger.LogInformation("Serviço 'Serviço de Livro': Iniciando busca por possíveis relacionamentos do Livro");
            using (IRepositorioGenerico<FichaEmprestimoAluno> servico = new EFRepositorioGenerico<FichaEmprestimoAluno>(_contexto))
            {
                livroEmUso = servico.ObtenhaDbSet().AsNoTracking().Any(x => x.FichaEmprestimoItens.Any(x => x.Livro.Id == Id || x.LivroId == Id));
            }

            if (!livroEmUso) 
            {
                try
                {
                    _logger.LogInformation("Serviço 'Serviço de Livro': Iniciando a remoção do Livro");
                    base.Delete(Id);
                    _logger.LogInformation("Serviço 'Serviço de Livro': Livro removido.");
                    return new InconsistenciaDeValidacaoTipado<Livro>() { Mensagem = $"Livro de código {Id.ToString("0000000")} foi deletado com sucesso" };
                }
                catch (Exception ex)
                {

                    _logger.LogError("Serviço 'Serviço de Livro': Livro não removido.", ex);
                    return new InconsistenciaDeValidacaoTipado<Livro>() { Mensagem = $"Livro de código {Id.ToString("0000000")} não deletado com sucesso, erro no sistema" };
                }
               
            }

            _logger.LogInformation("Serviço 'Serviço de Livro': Livro está em uso por alguma ficha de emprestimo.");
            return new InconsistenciaDeValidacaoTipado<Livro>() { Mensagem = $"Livro de código {Id.ToString("0000000")} não foi deletado com sucesso, pois está relacionado a Ficha de Alunos" };
        }

        public Livro ObtenhaLivro(int Id)
        {
            try
            {
                _logger.LogInformation("Serviço 'Serviço de Livro': Iniciando busca do Livro");
                var resultado = base.ObtenhaPorId(Id);
                if (!resultado.PossuiValor()) 
                {
                    _logger.LogInformation("Serviço 'Serviço de Livro': Livro não encontrado");
                    return null; 
                }

                _logger.LogInformation("Serviço 'Serviço de Livro': Fim busca do Livro, devolvendo resultado");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Livro': Busca do Livro apresentou erro", ex);
                return null;
            }
            
        }

        public IList<Livro> ObtenhaTodosLivros()
        {
            try
            {
                _logger.LogInformation("Serviço 'Serviço de Livro': Iniciando busca dos Livros");
                IList<Livro> livros = new List<Livro>();
                
                var resultado = base.ObtenhaDbSet().Include(x => x.Editora).Include(a => a.Autores).ToList();
                if (!resultado.PossuiValor() && !resultado.PossuiLinhas())
                {
                    _logger.LogInformation("Serviço 'Serviço de Livro': Livros não encontrados");
                    return null;
                }

                _logger.LogInformation("Serviço 'Serviço de Livro': Fim busca do Livros, devolvendo resultados");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço 'Serviço de Livro': Busca dos Livros apresentou erro", ex);
                return null;
            }
        }
    }
}
