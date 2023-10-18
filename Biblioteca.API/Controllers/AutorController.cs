using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/v1")]
    public class AutorController : PrincipalController
    {
        private readonly IServicoAutor _servicoAutor;

        public AutorController(ILogger<AutorController> logger, IServicoAutor servicoAutor)
        {
            _logger = logger;
            _servicoAutor = servicoAutor;
        }

        [Authorize]
        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar(CadastrarAutorDto dto)
        {
            var retorno = _servicoAutor.Cadastrar(dto);
            return RespostaResponalizada(retorno);
        }

        [Authorize]
        [HttpPut("Atualizar")]
        public IActionResult Atualizar(AlterarAutorDto dto)
        {
            var retorno = _servicoAutor.Atualizar(dto);
            return RespostaResponalizada(retorno);
        }

        [Authorize]
        [Authorize(Roles = Permissoes.ADMINISTRADOR)]
        [HttpDelete("Deletar{id}")]
        public IActionResult Atualizar(int id)
        {
            var retorno = _servicoAutor.Deletar(id);
            return RespostaResponalizada(retorno);
        }

        [Authorize]
        [HttpGet("ObterPorId/{id}")]
        public IActionResult ObterPorId(int id)
        {
            var retorno = _servicoAutor.ObterPorId(id);
            return RespostaResponalizada(retorno);
        }

        [Authorize]
        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var retorno = _servicoAutor.ObterTodos();
            return RespostaResponalizada(retorno);
        }

    }
}
