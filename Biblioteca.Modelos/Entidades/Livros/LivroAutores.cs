using Biblioteca.Negocio.Entidades.Autores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Livros
{
    public class LivroAutores : EntidadeBase
    {

        public Guid Codigo { get; set; }

       
        public int LivroId {  get; set; }

        public int AutorId { get; set; }
       
        public virtual Livro Livro { get; set; }

        public virtual Autor Autor { get; set; }
    }
}
