using Biblioteca.Negocio.Dtos.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using Biblioteca.Negocio.Utilidades.Conversores;
using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Negocio.Entidades.FichaEmprestimos
{
    public class FichaEmprestimoAluno : EntidadeBase
    {
        private Conversor<FichaEmprestimoAluno, FichaEmprestimoAlunoDto> _Conversor;
        public FichaEmprestimoAluno()
        {
            _Conversor = new Conversor<FichaEmprestimoAluno, FichaEmprestimoAlunoDto>();
        }
        public Guid Codigo { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int AlunoId { get; set; }

        public Aluno Aluno { get; set; }

        public string Observacoes { get; set; }

        public FichaEmprestimoAlunoStatusEnum  StatusEmprestimo { get; set; }

        public IList<FichaEmprestimoItem> FichaEmprestimoItens { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public DateTime DataEntregaEmprestimo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public FichaEmprestimoAlunoDto ObtenhaDto() => _Conversor.ConvertaPara(this);
    }
}
