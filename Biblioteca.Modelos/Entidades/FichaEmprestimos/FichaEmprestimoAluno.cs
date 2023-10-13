using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.FichaEmprestimos
{
    public class FichaEmprestimoAluno : EntidadeBase
    {
        public Guid Codigo { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int AlunoId { get; set; }

        public Aluno Aluno { get; set; }

        public string Observacoes { get; set; }

        public FichaEmprestimoAlunoStatusEnum  StatusEmprestimo { get; set; }

        public IList<FichaEmprestimoItem> FichaEmprestimoItens { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public DateTime DataVencimentoEmprestimo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
