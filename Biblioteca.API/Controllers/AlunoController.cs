using Biblioteca.Negocio.Dtos.Alunos;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("Alunos")]
    public class AlunoController : ControladorAbstratoComContexto<AlunoController>
    {
        
        private readonly IServicoAluno  _servicoAluno;

        public AlunoController(IServicoAluno servicoAluno, ILogger<AlunoController> logger)
        {
            base._logger = logger;
            _servicoAluno = servicoAluno;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Cadastrar(AlunoDto dto) 
        {
            try
            {
                return Ok("");
            }
            catch (Exception)
            {
                return Ok("");
                throw;
            }
        }
    }
}
