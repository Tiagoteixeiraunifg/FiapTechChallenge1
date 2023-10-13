using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.Extensions.Logging;


namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoAlunoImpl : EFRepositorioGenerico<Aluno>, IAlunoRepositorio, IServicoAluno
    {

        private readonly ILogger<IServicoAluno> _logger;

        public ServicoAlunoImpl(ApplicationDbContext contexto, ILogger<IServicoAluno> logger) : base(contexto)
        {
            _logger = logger;
            base._contexto = contexto;
        }

        public Aluno AtualizeAluno(Aluno dto)
        {
            _logger.LogInformation("Iniciando a atualização do Aluno");
            try
            {
                base.Altere(dto);
                _logger.LogInformation("Aluno atualizado Corretamente");
                return base._DbSet.Where(x => x.Codigo == dto.Codigo).First();
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro na atualização do Aluno", ex);
                return null;
            }
        }

        public Aluno CadastreAluno(Aluno dto)
        {
            _logger.LogInformation("Iniciando o cadastro do Aluno");
            try
            {
                _logger.LogInformation("Ajustando datas do cadastro");
                dto.DataCriacao = DateTime.Now;
                dto.DataAtualizacao = DateTime.Now;
                _logger.LogInformation("Datas ajustadas no cadastro");
                
                 base.Cadastre(dto);
                _logger.LogInformation("Aluno cadastrado corretamente");
                return base._DbSet.Where(x => x.Codigo == dto.Codigo).First();
            }

            catch (Exception ex)
            {
                _logger.LogError("Erro no cadastro do Aluno", ex);
                return null;
            }   
        }
             

        public bool DeleteAluno(int Id)
        {
            _logger.LogInformation("Iniciando a remoção do Aluno");
            try
            { 
                var res = base.Delete(Id);
                _logger.LogInformation("Remoção do aluno executada");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao remover aluno.", ex);
                return false;
            }
            
        }

        public Aluno ObtenhaAluno(int Id)
        {
            _logger.LogInformation("Iniciando a busca do aluno");
            try
            {
                var res = base.ObtenhaPorId(Id);
                _logger.LogInformation("Aluno encontrado com sucesso.");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar obter aluno", ex);
                return null;
                
            }
            
        }

        public IList<Aluno> ObtenhaTodosAlunos()
        {
            _logger.LogInformation("Iniciando a busca dos alunos");
            try
            {
                var res = base.ObtenhaTodos();
                _logger.LogInformation("Alunos encontrados");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar obter a lista de alunos.",ex);
                return null;
            }
            
        }
    }
}
