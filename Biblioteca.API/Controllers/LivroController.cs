using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.Livros;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("Livros")]
    public class LivroController : PrincipalControllerTipado<Livro>
    {
        private readonly IServicoLivro _ServicoLivro;

        public LivroController(IServicoLivro servicoLivro, ILogger<LivroController> logger)
        {
            _logger = logger;
            _ServicoLivro = servicoLivro;
        }

        [HttpPost("Cadastrar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Cadastre(LivroDto dto) 
        {
            dto.TipoOperacaoDeDadosEnum = Negocio.Enumeradores.Livros.LivrosTipoOperacaoDeDadosEnum.CADASTRAR;
            var resposta = _ServicoLivro.Cadastrar(dto);
            return RespostaResponalizada(resposta);
        }

        [HttpPut("Atualizar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Atualize(LivroDto dto)
        {
            dto.TipoOperacaoDeDadosEnum = Negocio.Enumeradores.Livros.LivrosTipoOperacaoDeDadosEnum.ALTERAR;
            var resposta = _ServicoLivro.Cadastrar(dto);
            return RespostaResponalizada(resposta);
        }

        
        [HttpDelete("Deletar/{Id}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Delete(int id) 
        {
            var resposta = _ServicoLivro.Deletar(id);
            return RespostaResponalizada(resposta);
        }

        
        [HttpGet("Obtenha/{Id}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaLivro(int Id) 
        {
            var resposta = _ServicoLivro.ObtenhaLivro(Id);
            return RespostaResponalizada(resposta);
        }
        
        
        [HttpGet("ObtenhaTodos")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaTodosLivros() 
        {
            var resposta = _ServicoLivro.ObtenhaTodosLivros();
            return RespostaResponalizada(resposta);
        }
    }
}
