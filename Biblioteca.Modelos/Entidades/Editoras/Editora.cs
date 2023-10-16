
using Biblioteca.Negocio.Dtos.Editoras;
namespace Biblioteca.Negocio.Entidades.Editoras
{
    public class Editora : EntidadeBase
    {
        public Editora() { }
         
        public Guid Codigo { get; set; }

        public string Cnpj {  get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set;}

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
