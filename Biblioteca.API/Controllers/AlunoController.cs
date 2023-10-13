using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.Alunos;
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
    public class AlunoController : ControladorAbstratoComContexto<AlunoController>
    {
        
        private readonly IServicoAluno  _servicoAluno;

        public AlunoController(IServicoAluno servicoAluno, ILogger<AlunoController> logger)
        {
            base._logger = logger;
            _servicoAluno = servicoAluno;
        }

        [Authorize]
        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar(AlunoDto dto) 
        {
            try
            {
                AlunoDto retorno = null;
                AlunoValidador validador = new AlunoValidador();
                _logger.LogInformation("Iniciando Validação do Aluno");
                var inconsistencias = validador.ValideCadastroDeAluno(dto.ObtenhaEntidade());
                _logger.LogInformation("Validação do Concluída");
                if (inconsistencias.EhValido()) 
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
                    return StatusCode(200, new { Inconsistencias = inconsistencias.listaDeInconsistencias });
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
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(AlunoDto dto)
        {
            try
            {
                AlunoDto retorno = null;
                AlunoValidador validador = new AlunoValidador();
                _logger.LogInformation("Iniciando Validação do Aluno");
                var inconsistencias = validador.ValideAtualizacaoDeAluno(dto.ObtenhaEntidade());
                _logger.LogInformation("Validação do Concluída");
                if (inconsistencias.EhValido())
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
                    return StatusCode(200, new { Inconsistencias = inconsistencias.listaDeInconsistencias });
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



    }
}
