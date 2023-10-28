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
    [Route("Livros/v1")]
    public class LivroController : PrincipalControllerTipado<Livro>
    {
        private readonly IServicoLivro _ServicoLivro;

        public LivroController(IServicoLivro servicoLivro, ILogger<LivroController> logger)
        {
            _logger = logger;
            _ServicoLivro = servicoLivro;
        }

        /// <summary>
        /// Cadastra um Livro
        /// </summary>
        /// <param name="dto">DtoLivro</param>
        /// <returns>Dados Cadastrados</returns>
        [HttpPost("Cadastrar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Cadastre(LivroDto dto) 
        {
            dto.TipoOperacaoDeDadosEnum = Negocio.Enumeradores.Livros.LivrosTipoOperacaoDeDadosEnum.CADASTRAR;
            var resposta = _ServicoLivro.Cadastrar(dto);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Atualiza um Livro
        /// </summary>
        /// <param name="dto">DtoLivro</param>
        /// <returns>Dados Atualizados</returns>
        [HttpPut("Atualizar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Atualize(LivroDto dto)
        {
            dto.TipoOperacaoDeDadosEnum = Negocio.Enumeradores.Livros.LivrosTipoOperacaoDeDadosEnum.ALTERAR;
            var resposta = _ServicoLivro.Cadastrar(dto);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Remove um Livro por Id
        /// </summary>
        /// <param name="id">LivroId -> numérico</param>
        /// <returns>Resposta da Reqquisição</returns>
        [HttpDelete("Deletar/{Id}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Delete(int id) 
        {
            var resposta = _ServicoLivro.Deletar(id);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Obtem um Livro pelo Id
        /// </summary>
        /// <param name="Id">LivroId -> numérico</param>
        /// <returns></returns>
        [HttpGet("Obtenha/{Id}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaLivro(int Id) 
        {
            var resposta = _ServicoLivro.ObtenhaLivro(Id);
            return RespostaResponalizada(resposta);
        }
        
        /// <summary>
        /// Obtem uma Coleção de Livros
        /// </summary>
        /// <returns>Coleção de Livros</returns>
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
