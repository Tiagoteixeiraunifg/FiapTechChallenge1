using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.Alunos;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Negocio.Validacoes.Alunos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("Alunos/v1")]
    public class AlunoController : PrincipalController
    {
        
        private readonly IServicoAluno  _servicoAluno;

        public AlunoController(IServicoAluno servicoAluno, ILogger<AlunoController> logger)
        {
            base._logger = logger;
            _servicoAluno = servicoAluno;
        }

        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar(AlunoDto dto) 
        {
            try
            {
                AlunoDto retorno = null;
                if (dto.EhValidoCadastro()) 
                {
                    _logger.LogInformation("Iniciando Cadastro do Aluno");
                    var resposta = _servicoAluno.CadastreAluno(dto.ObtenhaEntidade());
                    if (resposta.PossuiValor()) 
                    {
                        _logger.LogInformation("Cadastro do Aluno Concluido");
                       retorno =  resposta.ObtenhaDto();
                    }
                    
                }
                else 
                {
                    _logger.LogInformation("Validação do Concluída com Inconsistencias");
                    return RespostaResponalizada(dto.RetornarInconsistencia());
                }

                return StatusCode(200, retorno);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro ao cadastrar o Aluno");
                return StatusCode(500, new {Erro = ex.Message, ExecpionStackTrace = ex.StackTrace});
                throw;
            }
        }

        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(AlunoDto dto)
        {
            try
            {
                AlunoDto retorno = null;
                if (dto.EhValidoAtualizacao())
                {
                    _logger.LogInformation("Iniciando atualização do Aluno");
                    var resposta = _servicoAluno.AtualizeAluno(dto.ObtenhaEntidade());
                    if (resposta.PossuiValor())
                    {
                        _logger.LogInformation("Atualização do Aluno Concluido");
                        retorno = resposta.ObtenhaDto();
                    }

                }
                else
                {
                    _logger.LogInformation("Atualização com Inconsistencias");
                    return RespostaResponalizada(dto.RetornarInconsistencia());
                }

                return StatusCode(200, retorno);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro ao atualizar o Aluno");
                return StatusCode(500, new { Erro = ex.Message, ExecpionStackTrace = ex.StackTrace });
                throw;
            }
        }

        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [Authorize(Roles = Permissoes.ADMINISTRADOR)]
        [HttpDelete("Deletar/{Id}")]
        public IActionResult Deletar(int Id)
        {
            try
            {
                _logger.LogInformation("Iniciando a busca das informações");
                var resultado = _servicoAluno.ObtenhaAluno(Id);
                if (resultado.PossuiValor())
                {

                    _logger.LogInformation("Deletando o aluno");

                    var res = _servicoAluno.DeleteAluno(Id);

                    return res.PossuiValor()
                        ? StatusCode(204, new { Informacao = res })
                        : StatusCode(200, new { Informacao = "Aluno deletado" });

                }
                else
                {
                    _logger.LogInformation("Sem informacoes na busca das informações");
                    return StatusCode(204, new { Informacao = "Não foi encontrado registros" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao deletar informações", ex);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObtenhaTodosAlunos")]
        public IActionResult ObtenhaTodosAlunos() 
        {
            try
            {
                _logger.LogInformation("Iniciando a busca das informações");
                var resposta = _servicoAluno.ObtenhaTodosAlunos();


                if (resposta.PossuiValor() && resposta.PossuiLinhas()) 
                {
                    _logger.LogInformation("Convertendo as informações");
                    List<AlunoDto> lista = new List<AlunoDto>();
                    resposta.ToList().ForEach(x => lista.Add(x.ObtenhaDto()));

                    _logger.LogInformation("Retornando as informações");
                    return StatusCode(200, lista);
                }
                else 
                {
                    _logger.LogInformation("Sem informações");
                    return StatusCode(204, new { Inconsistencia = "Sem dados para retornar." });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no serviço de busca", ex);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObtenhaAluno/{Id}")]
        public IActionResult ObtenhaAluno(int Id) 
        {
            try
            {
                if (!Id.PossuiValor()) 
                {
                    return StatusCode(500, new { Erro = "Necessário informar o ID" });
                }


                _logger.LogInformation("Iniciando a busca das informações");
                var resposta = _servicoAluno.ObtenhaAluno(Id);


                if (resposta.PossuiValor())
                {
                    _logger.LogInformation("Convertendo as informações");
                    _logger.LogInformation("Retornando as informações");
                    return StatusCode(200, resposta.ObtenhaDto());
                }
                else
                {
                    _logger.LogInformation("Sem informações");
                    return StatusCode(204, new { Inconsistencia = "Sem dados para retornar." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro no serviço de busca", ex);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

    }
}
