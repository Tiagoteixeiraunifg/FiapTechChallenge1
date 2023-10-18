using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Runtime.CompilerServices;

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
            _logger.LogInformation("Serviço: Iniciando a atualização do Aluno");
            try
            {
                base.Altere(ObtenhaAlunoParaAtualizacao(dto));
                _logger.LogInformation("Serviço: Aluno atualizado Corretamente");
                return base._DbSet.Where(x => x.Codigo == dto.Codigo).First();
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço: Erro na atualização do Aluno", ex);
                return null;
            }
        }

        public Aluno CadastreAluno(Aluno dto)
        {
            _logger.LogInformation("Serviço: Iniciando o cadastro do Aluno");
            try
            {
                _logger.LogInformation("Serviço: Ajustando datas do cadastro");
                dto.DataCriacao = DateTime.Now;
                dto.DataAtualizacao = DateTime.Now;
                _logger.LogInformation("Serviço: Datas ajustadas no cadastro");
                _logger.LogInformation("Serviço: Criando código Guid no cadastro");
                dto.Codigo = Guid.NewGuid();

                base.Cadastre(dto);
                _logger.LogInformation("Serviço: Aluno cadastrado corretamente");
                return base._DbSet.Where(x => x.Codigo == dto.Codigo).First();
            }

            catch (Exception ex)
            {
                _logger.LogError("Serviço: Erro no cadastro do Aluno", ex);
                return null;
            }   
        }
             

        public List<string> DeleteAluno(int Id)
        {
            _logger.LogInformation("Serviço: Iniciando a remoção do Aluno");
            try
            {

                _logger.LogInformation("Serviço: Iniciando a busca por fichas de emprestimo do Aluno.");
                using (IRepositorioGenerico<FichaEmprestimoAluno> ser = new EFRepositorioGenerico<FichaEmprestimoAluno>(_contexto))
                {
                    var resposta = ser.ObtenhaDbSet().AsNoTracking().Where(x => x.AlunoId == Id || x.Aluno.Id == Id).ToList();
                    if(resposta.PossuiValor() && resposta.PossuiLinhas()) 
                    {
                        List<string> lista = new List<string>();
                        resposta.ToList().ForEach(x => lista.Add($"Ficha de Emprestimo Código: {x.Id.ToString("00000000")}"));
                        return lista;
                    }
                }


                var res = base.Delete(Id);
                _logger.LogInformation("Serviço: Remoção do aluno executada");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço: Erro ao remover aluno.", ex);
                return null;
            }
            
        }

        public Aluno ObtenhaAluno(int Id)
        {
            _logger.LogInformation("Serviço: Iniciando a busca do aluno");
            try
            {
                var res = base.ObtenhaPorId(Id);
                _logger.LogInformation("Serviço: Aluno encontrado com sucesso.");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço: Erro ao tentar obter aluno", ex);
                return null;
                
            }
            
        }

        public IList<Aluno> ObtenhaTodosAlunos()
        {
            _logger.LogInformation("Serviço: Iniciando a busca dos alunos");
            try
            {
                var res = base.ObtenhaTodos();
                _logger.LogInformation("Serviço: Alunos encontrados");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Serviço: Erro ao tentar obter a lista de alunos.", ex);
                return null;
            }
            
        }

        private Aluno ObtenhaAlunoParaAtualizacao(Aluno dto) 
        {

            ///AsNoTracking() usado para não gerar o Cache no contexto atual.
            ///
            var AlunoAtual = base._DbSet.AsNoTracking().Where(x => x.Id == dto.Id).First();
   

            dto.Id = dto.Id;
            dto.Codigo = AlunoAtual.Codigo;
            dto.Nome = AlunoAtual.Nome != dto.Nome ? dto.Nome : AlunoAtual.Nome;
            dto.Email = AlunoAtual.Email != dto.Email ? dto.Email : AlunoAtual.Email;
            dto.Telefone = AlunoAtual.Telefone != dto.Telefone ? dto.Telefone : AlunoAtual.Telefone;
            dto.DataCriacao = AlunoAtual.DataCriacao;
            dto.DataAtualizacao = DateTime.Now;

            return dto;

        }


    }
}
