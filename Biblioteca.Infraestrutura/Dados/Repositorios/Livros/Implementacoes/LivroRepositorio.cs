using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Negocio.Entidades.Livros;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Implementacoes
{
    public class LivroRepositorio : EFRepositorioGenerico<Livro>, ILivroRepositorio
    {
        public LivroRepositorio(ApplicationDbContext contexto) : base(contexto)
        {

        }

        public void CadastrarLivros(IList<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!_DbSet.Where(x => x.Codigo == livro.Codigo).Any())
                {
                    _DbSet.Add(livro);
                }
            }
            _contexto.SaveChanges();
        }

        public Livro ConsultarLivroPorIdEditar(int idEditora)
        {
            return _DbSet.Where(x => x.EditoraId == idEditora).FirstOrDefault();
        }

    }
}
