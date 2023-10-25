using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos
{
    public class FichaEmprestimoAlunoItensDto : EntidadeBase
    {
        public int LivroId { get; set; }

        public decimal Quantidade {  get; set; }

        public FichaEmprestimoAlunoItensStatusEnum StatusItem { get; set; }

    }
}
