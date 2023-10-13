using Biblioteca.Negocio.Entidades.Livros;
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

    }
}
