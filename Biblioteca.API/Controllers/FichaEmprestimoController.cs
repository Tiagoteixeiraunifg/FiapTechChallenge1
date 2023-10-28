using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("FichaEmprestimos/v1")]
    public class FichaEmprestimoController : PrincipalControllerTipado<FichaEmprestimoAluno>
    {
        private readonly IServicoFichaEmprestimoAluno _ServicoFicha;
        private const int LIMITE_DE_REGISTRO_PADRAO = 100;

        public FichaEmprestimoController(ILogger<FichaEmprestimoController> logger,IServicoFichaEmprestimoAluno servicoFicha)
        {
            _logger = logger;
            _ServicoFicha = servicoFicha;
        }

        /// <summary>
        /// Cadastra a Ficha de Emprestimo do Aluno
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Ficha cadastrada do Aluno</returns>
        ///<remarks>
        ///JSON - Objeto para cadastro padrão.
        ///ENUMERADORES: StatusItem => (1-ENTREGUE, 2-A_ENTREGAR), statusEmprestimoDescricao: texto (NORMAL, ATRASADA, ENTREGUE)
        /// </remarks>
        [HttpPost("Cadastrar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Cadastrar(FichaEmprestimoAlunoDto dto) 
        {
            var resposta = _ServicoFicha.CadastreFicha(dto);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Finalizar a Ficha de Emprestimo do Aluno
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns>Objeto da Ficha de Emprestimo</returns>
        /// <remarks>
        /// Parametro: Id tipo numérico
        /// </remarks>
        [HttpPost("Finalizar/{FichaId:int}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Finalizar(int FichaId) 
        {
            var ficha = _ServicoFicha.ObtenhaFichaPorCodigo(FichaId);
            if (!ficha.PossuiValor()) { return RespostaResponalizada(ficha); }


            var resposta = _ServicoFicha.FinalizeFicha(ficha);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Executa a entrega de um livro contida em uma Ficha de Emprestimo
        /// </summary>
        /// <param name="LivroId"></param>
        /// <param name="FichaId"></param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("EntregarLivro")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult EntregarLivro([FromQuery]int LivroId, [FromQuery] int FichaId) 
        {
            var resposta = _ServicoFicha.ExecuteEntregaDeLivro(LivroId, FichaId);
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// Exclui a Ficha de Emprestimo
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("ExcluirFicha/{FichaId:int}")]
        [Authorize]
        public IActionResult ExcluirFicha(int FichaId) 
        {
            var resposta = _ServicoFicha.ExcluaFicha(FichaId);  
            return RespostaResponalizada( resposta);
        }

        /// <summary>
        /// Obtem todas as fichas
        /// </summary>
        /// <returns>Coleção com todas as fichas</returns>
        [HttpGet("ObtenhaTodasFichas")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaTodasFichas() 
        {
            var resposta = _ServicoFicha.ObtenhaTodasFichas();
            return RespostaResponalizada(resposta);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataInicio"></param>
        /// <param name="DataFim"></param>
        /// <param name="AlunoId"></param>
        /// <param name="Situacao"></param>
        /// <returns></returns>
        [HttpGet("ObtenhaFichasDoAlunoNoIntervalo")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaTodosFichasDoAlunoNoIntervalo([FromQuery] string DataInicio, 
                                                                  [FromQuery] string DataFim, 
                                                                  [FromQuery] int AlunoId, 
                                                                  [FromQuery] int Situacao) 
        {

            DateTime dateIni;
            DateTime dateFim;

            if (!DateTime.TryParse(DataInicio, out dateIni) || !DateTime.TryParse(DataFim, out dateFim))
            {
                return RespostaResponalizada(null);
            }

            var resposta = _ServicoFicha.ObtenhaFichasDoAlunoPorCodigoEhIntervaloEhSituacao(AlunoId, dateIni, dateFim, (FichaEmprestimoAlunoStatusEnum)Situacao, LIMITE_DE_REGISTRO_PADRAO);
            return RespostaResponalizada(resposta);
        }


        /// <summary>
        /// Obtem as fichas no intervalo em atraso de entrega a 8 dias
        /// </summary>
        /// <param name="DataInicio"></param>
        /// <param name="DataFim"></param>
        /// <returns>Coleção com Fichas em atraso</returns>
        [HttpGet("ObtenhaFichasNoIntervaloEmAtraso")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaFichasNoIntervaloEmAtraso([FromQuery] string DataInicio,
                                                                 [FromQuery] string DataFim)
        {

            DateTime dateIni;
            DateTime dateFim;

            if (!DateTime.TryParse(DataInicio, out dateIni) || !DateTime.TryParse(DataFim, out dateFim))
            {
                return RespostaResponalizada(null);
            }

            var resposta = _ServicoFicha.ObtenhaFichasEmAtrasoDeEntregaPorIntervalo(dateIni, dateFim, LIMITE_DE_REGISTRO_PADRAO);
            return RespostaResponalizada(resposta);
        }


        /// <summary>
        /// Obtem as fichas do aluno
        /// </summary>
        /// <param name="AlunoId"></param>
        /// <returns>Fichas do aluno</returns>
        /// <remarks>Parametro: Id tipo numérico</remarks>
        [HttpGet("ObtenhaAsFichasDoAluno/{AlunoId:int}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaAsFichasDoAluno(int AlunoId) 
        {
            var resposta = _ServicoFicha.ObtenhaFichasDoAlunoPorCodigo(AlunoId, LIMITE_DE_REGISTRO_PADRAO);
            return RespostaResponalizada(resposta);

        }

        /// <summary>
        /// Obtem a ficha
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns>Ficha Emprestimo</returns>
        /// <remarks>Parametro: Id tipo numérico</remarks>
        [HttpGet("ObtenhaFicha/{FichaId:int}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaFicha(int FichaId)
        {
            var resposta = _ServicoFicha.ObtenhaFichaPorCodigo(FichaId);
            return RespostaResponalizada(resposta);

        }

    }
}
