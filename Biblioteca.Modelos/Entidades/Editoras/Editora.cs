using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Editoras
{
    public class Editora : EntidadeBase
    {
        public Guid Codigo { get; set; }

        public string Cnpj {  get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set;}

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
