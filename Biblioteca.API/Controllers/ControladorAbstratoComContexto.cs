using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    public abstract class ControladorAbstratoComContexto<T> : Controller 
        where T : class
    {
        protected ILogger<T> _logger;

    }


}
