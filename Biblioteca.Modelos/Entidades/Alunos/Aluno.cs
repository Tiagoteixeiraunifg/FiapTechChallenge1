using Biblioteca.Negocio.Dtos.Alunos;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Utilidades.Conversores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Alunos
{
    public class Aluno : EntidadeBase
    {

        private Conversor<Aluno, AlunoDto> _Conversor;

        public Aluno()
        {
            _Conversor = new Conversor<Aluno, AlunoDto>();
        }

        public Guid Codigo { get; set; } 
        
        public string Nome { get; set; }
        
        public string Email { get; set; }

        public string Telefone { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
        

        public AlunoDto ObtenhaDto() 
        {
            return _Conversor.ConvertaPara(this);
        }
 
    }
}
