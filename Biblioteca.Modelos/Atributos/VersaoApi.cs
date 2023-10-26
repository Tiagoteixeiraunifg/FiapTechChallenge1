using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Atributos
{
    /// <summary>
    /// Específica a versão da API na resposta da requisição do método anotado com o atributo.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class VersaoApi : Attribute
    {
        public string VersaoDaApi { get; set; }

        public VersaoApi()
        {
            
        }

        public VersaoApi(string versaoDaApi)
        {
            VersaoDaApi = versaoDaApi;
        }
    }
}
