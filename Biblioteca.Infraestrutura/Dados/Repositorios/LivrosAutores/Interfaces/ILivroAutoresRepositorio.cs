using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Negocio.Entidades.Livros;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Interfaces
{
    public interface ILivroAutoresRepositorio : IRepositorioGenerico<LivroAutores>
    {
        IList<LivroAutores> ConsultarLivroAutoresPorIdAutor(int id);
    }
}
