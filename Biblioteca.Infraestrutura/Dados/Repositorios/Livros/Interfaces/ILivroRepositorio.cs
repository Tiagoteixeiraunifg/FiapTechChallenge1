using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Negocio.Entidades.Livros;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces
{
    public interface ILivroRepositorio : IRepositorioGenerico<Livro>
    {
        void CadastrarLivros(List<Livro> livros);
    }
}
