using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Entidades.Usuarios;
using System.Text.Json;


namespace Biblioteca.Infraestrutura.Dados.ServicoDados.Implementacoes
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILivroRepositorio _livroRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public DataService(ApplicationDbContext applicationDbContext, ILivroRepositorio livroRepositorio, IUsuarioRepositorio usuarioRepositorio )
        {
            _applicationDbContext = applicationDbContext;
            _livroRepositorio = livroRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }
          
        public void InicializarBd()
        {
            _applicationDbContext.Database.EnsureCreated();

            IList<Livro> livros = PegarLivros();
            IList<Usuario> usuarios = PegarUsuarios();

            _livroRepositorio.CadastrarLivros(livros);
            _usuarioRepositorio.CadastrarUsuarios(FormatarPropriedadesUsuario(usuarios));
        }


        private static List<Livro> PegarLivros()
        {       

            var json = File.ReadAllText("dados.json");

            var livros = JsonSerializer.Deserialize<List<Livro>>(json);
            return livros;
        }

        private static IList<Usuario> PegarUsuarios()
        {

            var json = File.ReadAllText("dadosUsuario.json");

            var usuarios = JsonSerializer.Deserialize<IList<Usuario>>(json);
            return usuarios;
        }

        private IList<Usuario> FormatarPropriedadesUsuario(IList<Usuario> usuarios)
        {
            foreach (var usuario in usuarios) {
                usuario.Codigo = Guid.NewGuid();
                usuario.Senha = UtilitarioDeCriptografia.Criptografe(usuario.Senha);
            }
            return usuarios;
        }


    }
}
