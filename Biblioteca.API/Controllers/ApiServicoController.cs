using Biblioteca.Infraestrutura.Ferramentas.Conversores;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Entidades.API;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;


namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("ApiServicos/V1")]
    public class ApiServicoController : PrincipalControllerTipado<ApiServicos>
    {

        private const string URL_API_FUNCTION = "https://bibliotecafcatrasados.azurewebsites.net/api/";

        private const string URI_HTTP_START = "FunctionNotificacao_HttpStart";

        private const int LIMITE_DE_REGISTRO_PADRAO = 100;

        private readonly IServicoFichaEmprestimoAluno _ServicoFicha;

        private readonly INotificacaoEmail _Notificacao;

        public ApiServicoController(ILogger<ApiServicoController> logger, IServicoFichaEmprestimoAluno servicoFicha, INotificacaoEmail notificacao)
        {
            _logger = logger;
            _ServicoFicha = servicoFicha;
            _Notificacao = notificacao;
        }

        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("ObtenhaFichas")]
        public IActionResult ObtenhaFichasEmAtrasoDe8Dias() 
        {
            try
            {
                var resultado = _ServicoFicha.ObtenhaFichasEmAtrasoDeEntrega(LIMITE_DE_REGISTRO_PADRAO);
                if(resultado.PossuiValor() && resultado.PossuiLinhas()) 
                {
                    var res = new ApiServicos()
                    {
                        Identificador = Guid.NewGuid(),
                        ServicoExecutado = "ObtenhaFichasEmAtrasoDeEntrega",
                        Propriedade = "FichaEmprestimoAluno",
                        DataExecucao = DateTime.Now,
                        GerouResultados = true,
                        ExecutaComSucesso = true,
                        Valor = resultado
                    };
                  
                    return Json(res);    
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var resp = new ApiServicos()
            {
                Identificador = Guid.NewGuid(),
                ServicoExecutado = "ObtenhaFichasEmAtrasoDeEntrega",
                Propriedade = "FichaEmprestimoAluno",
                DataExecucao = DateTime.Now,
                GerouResultados = false,
                ExecutaComSucesso = true,
                Valor = new List<FichaEmprestimoAluno>()
            };

            return Json(resp);

        }


        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpPost("NotifiqueAlunos")]
        public IActionResult NotifiqueAlunosComFichasAtrasadas(ApiServicos dados) 
        {
            try
            {
                if (dados.PossuiValor() && dados.ExecutaComSucesso && dados.GerouResultados) 
                {

                    var lista = ConversorJson.ConvertaJson<List<FichaEmprestimoAluno>>(dados.Valor.ToString());

                    if (lista.PossuiValor() && lista.PossuiLinhas()) 
                    {
                        var resultado = NotifiqueListaDeAlunosComAtraso(lista);
                        if (resultado) 
                        {
                            var re = new ApiServicos()
                            {
                                Identificador = Guid.NewGuid(),
                                ServicoExecutado = "NotifiqueListaDeAlunosComAtraso",
                                Propriedade = "FichaEmprestimoAluno",
                                DataExecucao = DateTime.Now,
                                GerouResultados = false,
                                ExecutaComSucesso = true,
                                Valor = null
                            };

                            return Json(re); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var resp = new ApiServicos()
            {
                Identificador = Guid.NewGuid(),
                ServicoExecutado = "NotifiqueListaDeAlunosComAtraso",
                Propriedade = "FichaEmprestimoAluno",
                DataExecucao = DateTime.Now,
                GerouResultados = false,
                ExecutaComSucesso = true,
                Valor = new List<FichaEmprestimoAluno>()
            };

            return Json(resp);


        }


        [AllowAnonymous]
        [VersaoApi(VersaoDaApi = "V1.0")]
        [HttpGet("InicieManutencaoAlunos")]
        public IActionResult InicieManutencaoAlunos() 
        {
            try
            {
                _logger.LogInformation("Iniciando a manutenção de fichas em atraso.");
                using (var cliente = new HttpClient())
                {

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    cliente.BaseAddress = new Uri(URL_API_FUNCTION);
                    cliente.DefaultRequestHeaders.Accept.Clear();
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    cliente.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );


                    var resposta = cliente.GetAsync(URI_HTTP_START).Result;
                    _logger.LogInformation("Fim da manutenção das fichas em atraso.");

                    if (!resposta.IsSuccessStatusCode)
                    {
                        _logger.LogError("Erro ao iniciar a manutenção das fichas em atraso.");
                        throw new Exception(string.Format("Codigo: {0} | Content: {1}", resposta.StatusCode, resposta.Content.ToString()));
                    }

                    var textoResposta = resposta.Content.ReadAsStringAsync().Result;
                    var statusQuery = JsonConvert.DeserializeObject<StatusQuery>(textoResposta);

                    _logger.LogInformation("Obtendo resultado das functions Azure");

                    var StatusDaExecucao = JsonConvert.DeserializeObject<ApiServicos>(ObtenhaStatusQueryPelaUrl(statusQuery));



                    return Json(StatusDaExecucao);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao acessar API", ex);
                return Json("");
            }

        }

        private bool NotifiqueListaDeAlunosComAtraso(List<FichaEmprestimoAluno> fichas) 
        {
            bool executado = false;

            foreach (var item in fichas)
            {
                if (item.Aluno.Email.PossuiValor()) 
                {
                    executado = _Notificacao.NotifiqueFichaAtraso8Dias(item);
                    if (!executado) 
                    {
                        return false;
                    }
                }

            }

            return executado;
        }

        private class StatusQuery 
        {
            public Guid Id { get; set; }

            public string statusQueryGetUri { get; set; }
        }

        private string ObtenhaStatusQueryPelaUrl(StatusQuery urlCompleta) 
        {
            string URI = urlCompleta.statusQueryGetUri.Replace(URL_API_FUNCTION, "");
            
            try
            {
                _logger.LogInformation("Iniciando a consulta de function em funcionamento.");
                using (var cliente = new HttpClient())
                {

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    cliente.BaseAddress = new Uri(URL_API_FUNCTION);
                    cliente.DefaultRequestHeaders.Accept.Clear();
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    cliente.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );


                    var resposta = cliente.GetAsync(URI).Result;
                    _logger.LogInformation("Fim da consulta de function em funcionamento.");

                    if (!resposta.IsSuccessStatusCode)
                    {
                        _logger.LogError("Erro ao obter as consulta de function em funcionamento.");
                        throw new Exception(string.Format("Codigo: {0} | Content: {1}", resposta.StatusCode, resposta.Content.ToString()));
                    }

                    var textoResposta = resposta.Content.ReadAsStringAsync().Result;
                    var anonymous = new { runtimeStatus = "", output = new ApiServicos() };

                    var apiServicos = JsonConvert.DeserializeAnonymousType(textoResposta, anonymous);

                    if (!apiServicos.runtimeStatus.Equals("Completed"))
                    {
                        return ObtenhaStatusQueryPelaUrl(urlCompleta);
                    }

                    return JsonConvert.SerializeObject(apiServicos.output);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao acessar API", ex);
                return "";
            }

        }
    }
}
