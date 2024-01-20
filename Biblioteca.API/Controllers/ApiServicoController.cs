using Biblioteca.Infraestrutura.Ferramentas.Conversores;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Atributos;
using Biblioteca.Negocio.Entidades.API;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Notificacoes.Emails.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("ApiServicos/V1")]
    public class ApiServicoController : PrincipalControllerTipado<ApiServicos>
    {

        private readonly IServicoFichaEmprestimoAluno _ServicoFicha;
        private readonly INotificacaoEmail _Notificacao;

        private const int LIMITE_DE_REGISTRO_PADRAO = 100;

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
    }
}
