using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Alunos
{
    public class Aluno : EntidadeBase
    {
        public Guid Codigo { get; set; } 
        
        public string Nome { get; set; }
        
        public string Email { get; set; }

        public string Telefone { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
        
    }
}
