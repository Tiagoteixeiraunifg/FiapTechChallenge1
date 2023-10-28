using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("Autores/v1")]
    public class AutorController : PrincipalController
    {
        private readonly IServicoAutor _servicoAutor;

        public AutorController(ILogger<AutorController> logger, IServicoAutor servicoAutor)
        {
            _logger = logger;
            _servicoAutor = servicoAutor;
        }

        /// <summary>
        /// Cadastra um Autor
        /// </summary>
        /// <param name="dto">DtoAutor</param>
        /// <returns>Autor Cadastrado</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar(CadastrarAutorDto dto)
        {
            var retorno = _servicoAutor.Cadastrar(dto);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Atualiza um Autor
        /// </summary>
        /// <param name="dto">DtoAutor</param>
        /// <returns>Autor Atualizado</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(AlterarAutorDto dto)
        {
            var retorno = _servicoAutor.Atualizar(dto);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Remove um Autor
        /// </summary>
        /// <param name="id">AutorId -> Numérico</param>
        /// <returns>Retorno da Requisição</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [Authorize(Roles = Permissoes.ADMINISTRADOR)]
        [HttpDelete("Deletar{id}")]
        public IActionResult Deletar(int id)
        {
            var retorno = _servicoAutor.Deletar(id);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Obtenha um Autor por Id
        /// </summary>
        /// <param name="id">AutorId -> numérico</param>
        /// <returns>Dados do Autor</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObterPorId/{id}")]
        public IActionResult ObterPorId(int id)
        {
            var retorno = _servicoAutor.ObterPorId(id);
            return RespostaResponalizada(retorno);
        }

        /// <summary>
        /// Obtem uma coleção com Todos Autores
        /// </summary>
        /// <returns>Coleção de Autores</returns>
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var retorno = _servicoAutor.ObterTodos();
            return RespostaResponalizada(retorno);
        }

    }
}
