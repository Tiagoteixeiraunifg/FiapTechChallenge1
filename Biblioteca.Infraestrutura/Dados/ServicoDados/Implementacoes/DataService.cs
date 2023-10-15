using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Interfaces;
using Biblioteca.Negocio.Entidades.Livros;


using System.Text.Json;


namespace Biblioteca.Infraestrutura.Dados.ServicoDados.Implementacoes
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILivroRepositorio _livroRepositorio;

        public DataService(ApplicationDbContext applicationDbContext, ILivroRepositorio livroRepositorio)
        {
            _applicationDbContext = applicationDbContext;
            _livroRepositorio = livroRepositorio;
        }
          
        public void InicializarBd()
        {
            _applicationDbContext.Database.EnsureCreated();

            List<Livro> livros = PegarLivros();

            _livroRepositorio.CadastrarLivros(livros);

         
        }


        private static List<Livro> PegarLivros()
        {       

            var json = File.ReadAllText("dados.json");

            var livros = JsonSerializer.Deserialize<List<Livro>>(json);
            return livros;
        }
       
    }
}
