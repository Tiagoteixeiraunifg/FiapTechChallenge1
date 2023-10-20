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
        [JsonIgnore]
        public Guid Codigo { get; set; }

        [JsonIgnore]
        public int LivroId {  get; set; }

        public int AutorId { get; set; }

        [JsonIgnore]
        public virtual Livro Livro { get; set; }

        [JsonIgnore]
        public virtual Autor Autor { get; set; }
    }
}
