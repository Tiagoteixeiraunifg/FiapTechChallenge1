using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Infraestrutura.Seguranca.JWT.Interfaces;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.Usuarios;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Validacoes.Usuarios;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace Biblioteca.API.Controllers
{

    /// <summary>
    /// Porta de entrada para autenticação.
    /// </summary>
    [ApiController]
    [Route("Autenticacao/v1")]
    public class AutenticacaoController : ControladorAbstratoComContexto<AutenticacaoController>
    {

        private readonly IServicoDeToken _servicoDeToken;
        private readonly IServicoUsuario _servicoUsuario;
        
        public AutenticacaoController(ILogger<AutenticacaoController> logger, IServicoDeToken servicoDeToken, IServicoUsuario servicoUsuario)
        {
            _logger = logger;
            _servicoDeToken = servicoDeToken;
            _servicoUsuario = servicoUsuario;
        }


        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("AutenticarUsuario")]
        public IActionResult Autentique(Usuario dto)
        {
            try
            {

                Usuario usuarioLogado = null;
                string token = "";


                _logger.LogInformation("Iniciando Validação do Usuário");
                AutenticacaoValidador validador = new AutenticacaoValidador();
                var validacoes = validador.ValideAutenticacao(dto);

                _logger.LogInformation("Iniciando Autenticação do Usuário");
                if (_servicoUsuario.AutentiqueUsuario(dto) && validacoes.EhValido())
                {
                    usuarioLogado = _servicoUsuario.ObtenhaTodosUsuarios().Where(x => x.Nome.ToLowerInvariant() == dto.Nome.ToLowerInvariant()).FirstOrDefault();
                    
                    _logger.LogInformation("Autenticação do Usuário Executada - Gerando Token");
                    
                    token = _servicoDeToken.GerarToken(usuarioLogado);

                    _logger.LogInformation($"Token Gerado = {token}");
                }
                else
                {
                    return StatusCode(403, new { Mensagem = "Usuário ou senha inválida.", Validacoes = validacoes.listaDeInconsistencias});
                }

                _logger.LogInformation("Usuario logado com sucesso");

                return Ok(new
                {
                    Usuario = usuarioLogado.ObtenhaDto(),
                    Token = token
                });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erro ao logar Usuario", new { ex.Message });
                return BadRequest(ex.Message);

            }

        }

        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("CadastrarUsuario")]
        public IActionResult Cadastre(Usuario dto) 
        {

            try
            {
                Usuario novoCadastro = null;
                _logger.LogInformation("Iniciando Validação do Usuário");
                AutenticacaoValidador validador = new AutenticacaoValidador();
                var validacoes = validador.ValideCadastro(dto);

                if (validacoes.EhValido()) 
                {
                    _logger.LogInformation("Iniciando Cadastro do Usuário");

                    _logger.LogInformation("Criptografando a Senha do Cadastro do Usuário");
                    dto.Senha = UtilitarioDeCriptografia.Criptografe(dto.Senha);
                    _logger.LogInformation("Criando Código Guid do Cadastro do Usuário");
                    dto.Codigo = Guid.NewGuid();
                    _logger.LogInformation("Gravando Cadastro do Usuário");
                    var resposta = _servicoUsuario.Cadastrar(dto);

                    if (resposta.PossuiValor()) 
                    {
                        novoCadastro = resposta;
                    }
                }
                else 
                {
                    _logger.LogInformation("Existe Inconsistências");
                    return StatusCode(502, new { Validacoes = validacoes.listaDeInconsistencias});
                }

                _logger.LogInformation("Retornando Cadastro do Usuário");
                return Ok(novoCadastro.ObtenhaDto());

            }
            catch (Exception ex)
            {
                _logger.LogError("Iniciando Validação do Usuário");
                return StatusCode(500, new { Erro = ex.Message });
            }


        }

        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("RenovarToken/{token}")]
        public IActionResult RenovarToken(string token) 
        {
            try
            {
                _logger.LogInformation("Iniciando validação de token");
                var tokenRenovado = _servicoDeToken.RenoveToken(token);
                if (tokenRenovado.IsNullOrEmpty()) 
                {
                    _logger.LogInformation("Retornando validação de token invalido.");
                    return StatusCode(403, new { Retorno = "O Token é invalido."});
                }

                _logger.LogInformation("Retornando token valído.");
                return Ok(new {Token = $"{tokenRenovado}" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Retornando erro no processo de renovação de token");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

    }
}
