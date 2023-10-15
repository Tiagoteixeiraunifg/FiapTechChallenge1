
using Biblioteca.Negocio.Dtos.Editoras;
namespace Biblioteca.Negocio.Entidades.Editoras
{
    public class Editora : EntidadeBase
    {

        public Editora() { }

        public Editora(CadastroEditoraDto editoraDto) {

            Codigo = Guid.NewGuid();
            Nome = editoraDto.Nome;
            Cnpj = editoraDto.Cnpj;
            Cidade = editoraDto.Cidade;
            DataCriacao = DateTime.Now;         
        }

        public  Editora Atualizar (AlterarEditoraDto editoraDto)
        {
            Id = editoraDto.Id;
            Codigo = Guid.NewGuid();
            Nome = editoraDto.Nome;
            Cnpj = editoraDto.Cnpj;
            Cidade = editoraDto.Cidade;
            DataAtualizacao = DateTime.Now;

            return this;
        }

    

        public Guid Codigo { get; set; }

        public string Cnpj {  get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set;}

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
