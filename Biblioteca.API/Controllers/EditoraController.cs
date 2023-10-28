using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{

    [ApiController]
    [Route("Editoras/v1")]
    public class EditoraController : PrincipalController
    {
        private readonly IServicoEditora _servicoEditora;

        public EditoraController(ILogger<EditoraController> logger, IServicoEditora servicoEditora)
        {
            _logger = logger;
            _servicoEditora = servicoEditora;
        }

        /// <summary>
        /// Cadastra uma Editora
        /// </summary>
        /// <param name="dto">DtoEditora</param>
        /// <returns>Dados Cadastrados</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar(CadastroEditoraDto dto)
        {
             var retorno = _servicoEditora.Cadastrar(dto);
              return RespostaResponalizada(retorno);           
        }

        /// <summary>
        /// Atualiza uma Editora 
        /// </summary>
        /// <param name="dto">DtoEditora</param>
        /// <returns>Dados Atualizados</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(AlterarEditoraDto dto)
        {
            var retorno = _servicoEditora.Atualizar(dto);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Remove um Editora
        /// </summary>
        /// <param name="id">EditoraId -> numérico</param>
        /// <returns>Resposta da Requisição</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpDelete("Deletar/{id}")]
        public IActionResult Deletar(int id)
        {
            var retorno = _servicoEditora.Deletar(id);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Obtem uma Editora por Id
        /// </summary>
        /// <param name="id">EditoraId -> numérico</param>
        /// <returns>Dados da Editora</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObterPorId/{id}")]
        public IActionResult ObterPorId(int id)
        {
            var retorno = _servicoEditora.ObterPorId(id);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Obtem uma coleção de Editoras
        /// </summary>
        /// <returns>Coleção de Editoras</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var retorno = _servicoEditora.ObterTodos();
            return RespostaResponalizada(retorno);
        }

    }
}
