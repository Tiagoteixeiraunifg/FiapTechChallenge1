using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FuncaoFluxoNotificacao
{
    public static class FunctionNotificacao
    {

        #region CONSTANTES

        private const string URL_API_BIBLIOTECA = "https://bibliotecaapicicd.azurewebsites.net/ApiServicos/V1/";

        private const string URI_BUSCAFICHAS = "ObtenhaFichas";

        private const string URI_NOTIFIQUEALUNOS = "NotifiqueAlunos";

        #endregion


        [FunctionName("FunctionNotificacao")]
        public static async Task<ApiServicos> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var respostaServicos = new ApiServicos();


            respostaServicos = await context.CallActivityAsync<ApiServicos>(nameof(BusqueFichas), "");
            if (respostaServicos.ExecutaComSucesso && respostaServicos.GerouResultados) 
            {
                var resNotificacao = await context.CallActivityAsync<ApiServicos>(nameof(NotifiqueAlunos), respostaServicos);
                if (resNotificacao.ExecutaComSucesso) 
                {
                    return resNotificacao;
                }
            }
            else 
            {
                return respostaServicos;
            }

            return respostaServicos;
        }



        [FunctionName("BusqueFichas")]
        public static ApiServicos BusqueFichas([ActivityTrigger] string dados, ILogger log)
        {
            var saida = new ApiServicos();

            try
            {
                log.LogInformation("Iniciando a consulta de fichas em atraso.");
                using (var cliente = new HttpClient())
                {

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    cliente.BaseAddress = new Uri(URL_API_BIBLIOTECA);
                    cliente.DefaultRequestHeaders.Accept.Clear();
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    cliente.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );


                    var resposta = cliente.GetAsync(URI_BUSCAFICHAS).Result;
                    log.LogInformation("Fim da consulta de fichas em atraso.");

                    if (!resposta.IsSuccessStatusCode)
                    {
                        log.LogError("Erro ao obter as fichas em atraso.");
                        throw new Exception(string.Format("Codigo: {0} | Content: {1}", resposta.StatusCode, resposta.Content.ToString()));
                    }

                    var textoResposta = resposta.Content.ReadAsStringAsync().Result;
                    var apiServicos = JsonConvert.DeserializeObject<ApiServicos>(textoResposta);


                    return apiServicos;
                }
            }
            catch (Exception ex)
            {
                log.LogError("Erro ao acessar API", ex);
                return saida;
            }
        }

        [FunctionName("NotifiqueAlunos")]
        public static ApiServicos  NotifiqueAlunos([ActivityTrigger] ApiServicos dados, ILogger log)
        {
            var saida = new ApiServicos();
            try
            {
                log.LogInformation("Iniciando a notificação de fichas em atraso.");
                using (var cliente = new HttpClient())
                {

                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    cliente.BaseAddress = new Uri(URL_API_BIBLIOTECA);
                    cliente.DefaultRequestHeaders.Accept.Clear();
                    cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                    cliente.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );

                    var corpoRequisicao = new StringContent(JsonConvert.SerializeObject(dados), Encoding.UTF8, "application/json");

                    var resposta = cliente.PostAsync(URI_NOTIFIQUEALUNOS, corpoRequisicao).Result;

                    if (!resposta.IsSuccessStatusCode)
                    {
                        log.LogError("Erro  na notificação de fichas em atraso.");
                        throw new Exception(string.Format("Codigo: {0} | Content: {1}", resposta.StatusCode, resposta.Content.ToString()));
                    }

                    var textoResposta = resposta.Content.ReadAsStringAsync().Result;
                    var apiServicos = JsonConvert.DeserializeObject<ApiServicos>(textoResposta);


                    return apiServicos;
                }
            }
            catch (Exception ex)
            {
                log.LogError("Erro ao acessar API", ex);
                return saida;
            }

        }


        [FunctionName("FunctionNotificacao_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("FunctionNotificacao", null);

            log.LogInformation($"Iniciado orquestrador com a ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }



        public class ApiServicos
        {
            public Guid Identificador { get; set; }

            public DateTime DataExecucao { get; set; }

            public string ServicoExecutado { get; set; }

            public bool ExecutaComSucesso { get; set; }

            public bool GerouResultados { get; set; }

            public string Propriedade { get; set; }

            public object Valor { get; set; }

        }



    }
}