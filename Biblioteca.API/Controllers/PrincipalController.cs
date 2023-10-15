using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    public class PrincipalController : ControladorAbstratoComContexto<PrincipalController>
    {
        protected ICollection<string> Erros = new List<string>();

        protected IActionResult RespostaResponalizada(InconsistenciaDeValidacao inconsistenciaDeValidacao)
        {          
            AdicionarErroProcessamento(inconsistenciaDeValidacao);

            if (inconsistenciaDeValidacao.EhValido())  return Ok(new { inconsistenciaDeValidacao.Mensagem });

            return StatusCode(200, new { Erros });
        }

        protected IActionResult RespostaResponalizada(object objeto = null)
        {
            if (OperacaoValida()) return Ok(objeto);                      

            return StatusCode(200, new { Erros });
        }

        protected void AdicionarErroProcessamento(InconsistenciaDeValidacao inconsistenciaDeValidacao) 
        {
            foreach (var error in inconsistenciaDeValidacao.listaDeInconsistencias)
            {
                Erros.Add(error.Mensagem.ToString());
            }
        }

        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }
    }
}
