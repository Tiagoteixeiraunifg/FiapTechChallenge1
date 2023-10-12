using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Enumeradores.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Livros
{
    public class Livro : EntidadeBase
    {
        public Guid Codigo { get; set; }

        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public decimal QuantidadeEstoque { get; set; }

        public StatusLivroEnum Status { get; set; }

        public IList<Autor> Autores { get; set; }

        public Editora  Editora { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

    }
}
