using Biblioteca.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Dtos.Livros
{
    public class LivrosAutoresDto : EntidadeBase
    {
        public int AutorId { get; set; }

    }
}
