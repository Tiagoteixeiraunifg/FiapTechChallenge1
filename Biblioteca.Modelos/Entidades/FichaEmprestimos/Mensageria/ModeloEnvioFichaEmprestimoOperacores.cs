using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.FichaEmprestimos.Mensageria
{
    public class ModeloEnvioFichaEmprestimoOperacores
    {
        public FichaEmprestimoAlunoTipoOperacaoAsyncEnum Operacao { get; set; }

        public DateTime Data { get; set; }

        public string Ficha { get; set; }

        public string Operador { get; set; }
    }
}
