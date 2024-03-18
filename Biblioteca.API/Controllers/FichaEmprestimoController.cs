using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Infraestrutura.Seguranca.JWT.Interfaces;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos.Mensageria;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Servicos.Contratos.Servicos;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("FichaEmprestimos")]
    public class FichaEmprestimoController : PrincipalControllerTipado<FichaEmprestimoAluno>
    {
        private readonly IServicoFichaEmprestimoAluno _ServicoFicha;
        private const int LIMITE_DE_REGISTRO_PADRAO = 100;
        private IServicoMensageria _Mensageria;
        private IServicoDeToken _servicoToken;
        public FichaEmprestimoController(ILogger<FichaEmprestimoController> logger,IServicoFichaEmprestimoAluno servicoFicha, IServicoMensageria mensageria, IServicoDeToken servicoToken)
        {
            _logger = logger;
            _ServicoFicha = servicoFicha;
            _Mensageria = mensageria;
            _servicoToken = servicoToken;
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
        [HttpPost("v1/Cadastrar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult Cadastrar(FichaEmprestimoAlunoDto dto) 
        {
            var resposta = _ServicoFicha.CadastreFicha(dto);
            return RespostaResponalizada(resposta);
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
        [HttpPost("MicroServico/v2/Cadastrar")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V2.0")]
        public async Task<IActionResult> CadastrarV2(FichaEmprestimoAlunoDto dto)
        {
            if (!dto.PossuiValor()) { return StatusCode(500); }

            try
            {
                _logger.LogInformation("Inicializando a Cadastro da Ficha por MicroServiço.");
                _logger.LogInformation("Buscando o token da request.");

                var token = Request.Headers.Authorization.ToString();

                _logger.LogInformation("Buscando o usuario do token da request.");

                var usuario = _servicoToken.ObtenhaUsuarioDoToken(token);

                _logger.LogInformation($"Buscando o usuario do token da request concluído, usuário: {usuario.Nome}");

                var inconsistencias = _ServicoFicha.ValideCadastroFicha(dto);
                if (!inconsistencias.EhValido()) { return RespostaResponalizada(inconsistencias); }

                _logger.LogInformation($"Convertendo para JSON a ficha: {dto.Codigo}");
                
                var fichaSerializada = JsonConvert.SerializeObject(dto, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                _logger.LogInformation($"Convertendo para JSON o Usuario: Id - {usuario.Id}");

                var usuarioSerializado = JsonConvert.SerializeObject(usuario, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                _logger.LogInformation("Obtendo a Instancia do Enviador de Mensagens");
                var Enviador = _Mensageria.Instancia().Result;

                _logger.LogInformation("Enviando mensagem para Microserviço");
                await Enviador.Send<ModeloEnvioFichaEmprestimoOperacores>(new
                {
                    Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.CADASTRAR,
                    Data = DateTime.Now,
                    Ficha = fichaSerializada,
                    Operador = usuarioSerializado
                });

                _logger.LogInformation("Mensagem enviada com sucesso.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao enviar a mensagem para o serviço: {ex.Message}" + $" Pilha: {ex.StackTrace ?? ""}");
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Finalizar a Ficha de Emprestimo do Aluno
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns>Objeto da Ficha de Emprestimo</returns>
        /// <remarks>
        /// Parametro: Id tipo numérico
        /// </remarks>
        [HttpPost("v1/Finalizar/{FichaId:int}")]
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
        /// Finalizar a Ficha de Emprestimo do Aluno Alta Demanda
        /// </summary>
        /// <param name="FichaId"></param>
        /// <returns>Objeto da Ficha de Emprestimo</returns>
        /// <remarks>
        /// Parametro: Id tipo numérico
        /// </remarks>
        [HttpPost("MicroServico/v2/Finalizar/{FichaId:int}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V2.0")]
        public  async Task<IActionResult> FinalizarV2(int FichaId)
        {
            
            
            try
            {
                _logger.LogInformation("Inicializando a Finalização da Ficha por MicroServiço.");
                _logger.LogInformation("Buscando o token da request.");

                var token = Request.Headers.Authorization.ToString();

                _logger.LogInformation("Buscando o usuario do token da request.");

                var usuario = _servicoToken.ObtenhaUsuarioDoToken(token);

                _logger.LogInformation($"Buscando o usuario do token da request concluído, usuário: {usuario.Nome}");

                _logger.LogInformation("Obtendo a ficha pelo Id informado.");

                var ficha = _ServicoFicha.ObtenhaFichaPorCodigo(FichaId);

                if (!ficha.PossuiValor()) { return RespostaResponalizada(ficha); }

                _logger.LogInformation($"Obtendo a ficha pelo Id informado concluído, código: {ficha.Codigo}");

                _logger.LogInformation($"Convertendo para JSON a ficha: {ficha.Codigo}");
                var fichaSerializada = JsonConvert.SerializeObject(ficha, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                _logger.LogInformation($"Convertendo para JSON o Usuario: Id - {usuario.Id}");

                var usuarioSerializado = JsonConvert.SerializeObject(usuario, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                _logger.LogInformation("Obtendo a Instancia do Enviador de Mensagens");
                var Enviador = _Mensageria.Instancia().Result;

                _logger.LogInformation("Enviando mensagem para Microserviço");
                await Enviador.Send<ModeloEnvioFichaEmprestimoOperacores>(new ModeloEnvioFichaEmprestimoOperacores()
                {
                    Operacao = FichaEmprestimoAlunoTipoOperacaoAsyncEnum.FINALIZAR,
                    Data = DateTime.Now,
                    Ficha = fichaSerializada,
                    Operador = usuarioSerializado
                });

                _logger.LogInformation("Mensagem enviada com sucesso.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Erro ao enviar a mensagem para o serviço: {ex.Message}"+$" Pilha: {ex.StackTrace ?? ""}");
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// Executa a entrega de um livro contida em uma Ficha de Emprestimo
        /// </summary>
        /// <param name="LivroId"></param>
        /// <param name="FichaId"></param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("v1/EntregarLivro")]
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
        [HttpDelete("v1/ExcluirFicha/{FichaId:int}")]
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
        [HttpGet("v1/ObtenhaTodasFichas")]
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
        [HttpGet("v1/ObtenhaFichasDoAlunoNoIntervalo")]
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
        [HttpGet("v1/ObtenhaFichasNoIntervaloEmAtraso")]
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
        [HttpGet("v1/ObtenhaAsFichasDoAluno/{AlunoId:int}")]
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
        [HttpGet("v1/ObtenhaFicha/{FichaId:int}")]
        [Authorize]
        [VersaoApi(VersaoDaApi = "V1.0")]
        public IActionResult ObtenhaFicha(int FichaId)
        {
            var resposta = _ServicoFicha.ObtenhaFichaPorCodigo(FichaId);
            return RespostaResponalizada(resposta);

        }

    }
}
