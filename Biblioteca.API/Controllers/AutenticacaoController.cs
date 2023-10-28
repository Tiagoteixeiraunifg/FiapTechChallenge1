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
    public class AutenticacaoController : PrincipalControllerTipado<Usuario>
    {

        private readonly IServicoDeToken _servicoDeToken;
        private readonly IServicoUsuario _servicoUsuario;

        public AutenticacaoController(ILogger<AutenticacaoController> logger, IServicoDeToken servicoDeToken, IServicoUsuario servicoUsuario)
        {
            _logger = logger;
            _servicoDeToken = servicoDeToken;
            _servicoUsuario = servicoUsuario;
        }

        /// <summary>
        /// Autêntica um Usuário
        /// </summary>
        /// <param name="dto">Dados do Usuário</param>
        /// <returns>Token de Autenticação JWT</returns>
        /// <remarks>
        /// Observação: Deve informar apenas Nome e Senha.
        /// {    
        ///     "nome": "string",  
        ///     "senha": "string", 
        /// }
        /// </remarks>
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


                if (!validacoes.EhValido()) return RespostaResponalizada(validacoes);

                if (_servicoUsuario.AutentiqueUsuario(dto))
                {
                    usuarioLogado = _servicoUsuario.ObtenhaTodosUsuarios().Where(x => x.Nome.ToLowerInvariant() == dto.Nome.ToLowerInvariant()).FirstOrDefault();

                    _logger.LogInformation("Autenticação do Usuário Executada - Gerando Token");

                    token = _servicoDeToken.GerarToken(usuarioLogado);

                    _logger.LogInformation($"Token Gerado = {token}");
                }
                else
                {
                    return RespostaResponalizada(new Negocio.Validacoes.FabricaDeValidacoes.InconsistenciaDeValidacaoTipado<Usuario>() { Mensagem = "Usuario ou Senha não são validos."});
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


        /// <summary>
        /// Cadastra um usuário para Acesso do Sistema
        /// </summary>
        /// <param name="dto">Dados do Cadastro</param>
        /// <returns>Dados cadastrados</returns>
        /// <remarks>
        /// Observação: Permissões = (2 - OPERADOR, 1- ADMINISTRADOR )
        /// 
        /// {   
        ///     "nome": "string",  
        ///     "email": "string",  
        ///     "senha": "string",  
        ///     "permissao": 1
        /// }
        /// </remarks>
        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("CadastrarUsuario")]
        public IActionResult Cadastre(Usuario dto)
        {

            try
            {
                Usuario novoCadastro = null;
                _logger.LogInformation("Iniciando Validação do Usuário");
                var validador = new AutenticacaoValidador();
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
                    return RespostaResponalizada(validacoes);
                }

                _logger.LogInformation("Retornando Cadastro do Usuário");
                return Ok(novoCadastro.ObtenhaDto());

            }
            catch (Exception ex)
            {
                _logger.LogError("Iniciando Validação do Usuário");
                return RespostaResponalizada(new Negocio.Validacoes.FabricaDeValidacoes.InconsistenciaDeValidacaoTipado<AutenticacaoController>() { Mensagem = "Erro ao Cadastrar Usuario" });


            }
        }

        /// <summary>
        /// Renova o tokem passado como paramêtro
        /// </summary>
        /// <param name="token">Token para renovação</param>
        /// <returns>String Token Renovado</returns>
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
                    return StatusCode(403, new { Retorno = "O Token é invalido." });
                }

                _logger.LogInformation("Retornando token valído.");
                return Ok(new { Token = $"{tokenRenovado}" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Retornando erro no processo de renovação de token");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

    }
}
