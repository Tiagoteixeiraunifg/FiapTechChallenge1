using Biblioteca.Negocio.Entidades.Autores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Livros
{
    public class LivroAutores : EntidadeBase
    {
        public Guid Codigo { get; set; }

        public int LivroId {  get; set; }

        public Livro Livro { get; set; }

        public Autor Autor { get; set; }
    }
}
