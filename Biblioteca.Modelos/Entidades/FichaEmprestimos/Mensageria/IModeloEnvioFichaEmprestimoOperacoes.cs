using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.FichaEmprestimos.Mensageria
{
    public interface IModeloEnvioFichaEmprestimoOperacoes
    {
        public FichaEmprestimoAlunoTipoOperacaoAsyncEnum Operacao { get; }
        public DateTime Data { get; }
        public string Ficha { get; }

        public string Operador { get; }
        
    }
}
