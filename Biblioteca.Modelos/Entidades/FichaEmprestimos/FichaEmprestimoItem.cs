using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.FichaEmprestimos
{
    public class FichaEmprestimoItem : EntidadeBase
    {
        public Guid Codigo { get; set; }

        public int FichaEmprestimoAlunoId { get; set; }

        public FichaEmprestimoAluno FichaEmprestimoAluno { get; set; }

        public int LivroId { get; set; }

        public Livro Livro { get; set; }

        public decimal Quantidade { get; set; }

        public FichaEmprestimoAlunoItensStatusEnum StatusItem {  get; set; }

        public DateTime DataStatusItem { get; set; }

    }
}
