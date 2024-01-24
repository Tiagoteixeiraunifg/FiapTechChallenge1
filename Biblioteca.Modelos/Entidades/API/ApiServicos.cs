using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.API
{
    [Serializable]
    public class ApiServicos
    {
        public Guid Identificador { get; set; }

        public DateTime DataExecucao { get; set; }

        public string ServicoExecutado { get; set; }

        public bool ExecutaComSucesso { get; set; }

        public bool GerouResultados { get; set; }

        public string Propriedade { get; set; }

        public  object Valor { get; set; }

    }
}
