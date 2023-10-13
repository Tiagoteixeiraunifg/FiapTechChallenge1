using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.Usuarios;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Negocio.Validacoes.Usuarios;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("Usuario/v1")]
    public class UsuarioController : ControladorAbstratoComContexto<UsuarioController>
    {
        private readonly IServicoUsuario _servicoUsuario;

        public UsuarioController(ILogger<UsuarioController> logger, IServicoUsuario servicoUsuario)
        {
            _logger = logger;
            _servicoUsuario = servicoUsuario;
        }

        [Authorize]
        [Authorize(Roles = Permissoes.ADMINISTRADOR)]
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(Usuario dto)
        {
            try
            {
                Usuario novoCadastro = null;
                _logger.LogInformation("Iniciando Validação do Usuário");
                AutenticacaoValidador validador = new AutenticacaoValidador();
                var validacoes = validador.ValideCadastro(dto);

                if (validacoes.EhValido())
                {
                    _logger.LogInformation("Iniciando Atualizacao do Usuário");
                    _logger.LogInformation("Criptografando a Senha do Cadastro do Usuário");
                    dto.Senha = UtilitarioDeCriptografia.Criptografe(dto.Senha);
                    _logger.LogInformation("Gravando Cadastro Atualizado do Usuário");

                    var resposta = _servicoUsuario.AtualizeUsuario(dto);

                    if (resposta.PossuiValor())
                    {
                        novoCadastro = resposta;
                    }
                }
                else
                {
                    _logger.LogInformation("Existe Inconsistências");
                    return StatusCode(502, new { Validacoes = validacoes.listaDeInconsistencias });
                }

                _logger.LogInformation("Retornando Cadastro do Usuário");
                return Ok(novoCadastro.ObtenhaDto());

            }
            catch (Exception ex)
            {
                _logger.LogError("Devolvendo Exception");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [Authorize]
        [Authorize(Roles = Permissoes.ADMINISTRADOR)]
        [HttpDelete("Deletar/{Id}")]
        public IActionResult Deletar(int Id) 
        {
            try
            {
                _logger.LogInformation("Iniciando a busca das informações");
                var resultado = _servicoUsuario.ObtenhaTodosUsuarios();
                if (resultado.PossuiValor()
                    && resultado.PossuiLinhas()
                    && resultado.Any(x => x.Id == Id))
                {
                  
                    _logger.LogInformation("Deletando o usuario");

                    var res = _servicoUsuario.DeleteUsuario(Id);

                    return res 
                        ? StatusCode(200, new { Informacao = "Usuario deletado com sucesso" }) 
                        : StatusCode(500, new { Informacao = "Erro ao deletar o usuario." });

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
        [HttpGet("ObtenhaTodos")]
        public IActionResult ObtenhaTodos() 
        {
            try
            {
                _logger.LogInformation("Iniciando a busca das informações");
                var resultado = _servicoUsuario.ObtenhaTodosUsuarios();
                if (resultado.PossuiValor() && resultado.PossuiLinhas()) 
                {
                    IList<UsuarioDto> listaDeUsuarios = new List<UsuarioDto>();
                    _logger.LogInformation("Convertendo Lista de Usuarios em Lista de DtoUsuarios");
                    resultado.ToList().ForEach(x => listaDeUsuarios.Add(x.ObtenhaDto()));
                    _logger.LogInformation("Devolvendo Lista de Dto");
                    return StatusCode(200, listaDeUsuarios);
                }
                else 
                {
                    _logger.LogInformation("Sem informacoes na busca das informações");
                    return StatusCode(204, new { Informacao = "Não foi encontrado registros" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao obter informações", ex);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("ObtenhaUsuario/{Id}")]
        public IActionResult ObtenhaUsuario(int Id)
        {
            try
            {
                _logger.LogInformation("Iniciando a busca das informações");
                var resultado = _servicoUsuario.ObtenhaTodosUsuarios();
                if (resultado.PossuiValor() 
                    && resultado.PossuiLinhas()
                    && resultado.Any(x => x.Id == Id))
                {

                    _logger.LogInformation("Convertendo usuario em DtoUsuario");

                    var dto = resultado.FirstOrDefault(x => x.Id == Id).ObtenhaDto();

                    _logger.LogInformation("Devolvendo DtoUsuario");
                   
                    return StatusCode(200, dto);
                }
                else
                {
                    _logger.LogInformation("Sem informacoes na busca das informações");
                    return StatusCode(204, new { Informacao = "Não foi encontrado registros" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao obter informações", ex);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

    }
}
