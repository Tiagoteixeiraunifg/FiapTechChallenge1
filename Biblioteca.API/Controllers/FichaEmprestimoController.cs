using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("FichaEmprestimo")]
    public class FichaEmprestimoController : PrincipalControllerTipado<FichaEmprestimoAluno>
    {
        private readonly IServicoFichaEmprestimoAluno _ServicoFicha;
        private const int LIMITE_DE_REGISTRO_PADRAO = 100;

        public FichaEmprestimoController(ILogger<FichaEmprestimoController> logger,IServicoFichaEmprestimoAluno servicoFicha)
        {
            _logger = logger;
            _ServicoFicha = servicoFicha;
        }

        [HttpPost("Cadastrar")]
        
        public IActionResult Cadastrar(FichaEmprestimoAlunoDto dto) 
        {
            var resposta = _ServicoFicha.CadastreFicha(dto);
            return RespostaResponalizada(resposta);
        }

        [HttpPost("Finalizar")]
        public IActionResult Finalizar(FichaEmprestimoAlunoDto dto) 
        {
            var resposta = _ServicoFicha.FinalizeFicha(dto);
            return RespostaResponalizada(resposta);
        }

        [HttpPost("EntregarLivro")]
        public IActionResult EntregarLivro([FromQuery]int LivroId, [FromQuery] int FichaId) 
        {
            var resposta = _ServicoFicha.ExecuteEntregaDeLivro(LivroId, FichaId);
            return RespostaResponalizada(resposta);
        }

        [HttpDelete("ExcluirFicha/{FichaId:int}")]
        public IActionResult ExcluirFicha(int FichaId) 
        {
            var resposta = _ServicoFicha.ExcluaFicha(FichaId);  
            return RespostaResponalizada( resposta);
        }

        [HttpGet("ObtenhaTodasFichas")]
        public IActionResult ObtenhaTodasFichas() 
        {
            var resposta = _ServicoFicha.ObtenhaTodasFichas();
            return RespostaResponalizada(resposta);
        }


        [HttpGet("ObtenhaFichasDoAlunoNoIntervalo")]
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



        [HttpGet("ObtenhaFichasNoIntervaloEmAtraso")]
        public IActionResult ObtenhaFichasNoIntervaloEmAtraso([FromQuery] DateTime DataInicio,
                                                                 [FromQuery] DateTime DataFim)
        {
            var resposta = _ServicoFicha.ObtenhaFichasEmAtrasoDeEntregaPorIntervalo(DataInicio, DataFim, LIMITE_DE_REGISTRO_PADRAO);
            return RespostaResponalizada(resposta);
        }



        [HttpGet("ObtenhaAsFichasDoAluno/{AlunoId:int}")]
        public IActionResult ObtenhaAsFichasDoAluno(int AlunoId) 
        {
            var resposta = _ServicoFicha.ObtenhaFichasDoAlunoPorCodigo(AlunoId, LIMITE_DE_REGISTRO_PADRAO);
            return RespostaResponalizada(resposta);

        }

        [HttpGet("ObtenhaFicha/{FichaId:int}")]
        public IActionResult ObtenhaFicha(int FichaId)
        {
            var resposta = _ServicoFicha.ObtenhaFichaPorCodigo(FichaId);
            return RespostaResponalizada(resposta);

        }

    }
}
