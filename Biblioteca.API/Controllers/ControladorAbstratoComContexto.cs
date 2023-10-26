using Biblioteca.Negocio.Atributos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Biblioteca.API.Controllers
{
    public abstract class ControladorAbstratoComContexto<T> : Controller 
        where T : class
    {
        protected ILogger<T> _logger;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var objetos = context.ActionDescriptor.EndpointMetadata;

            foreach (var objeto in objetos) 
            {
                if (objeto.GetType() == (typeof(VersaoApi))) 
                {
                    var Atributo = (VersaoApi)objeto;                
                    base.HttpContext.Response.Headers.Add("VersaoAPI",$"{Atributo.VersaoDaApi}");
                }
            }



            base.OnActionExecuting(context);    
        }











    }
}
