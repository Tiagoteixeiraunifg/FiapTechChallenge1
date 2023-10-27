using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Negocio.Entidades.Usuarios;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes
{
    public class UsuarioRepositorioImpl : EFRepositorioGenerico<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorioImpl(ApplicationDbContext contexto) : base(contexto)
        {
            
        }

        public void CadastrarUsuarios(IList<Usuario> usuarios)
        {
            foreach (var usuario in usuarios)
            {
                if (!_DbSet.Where(x => x.Codigo == usuario.Codigo || x.Nome == usuario.Nome).Any())
                {
                    _DbSet.Add(usuario);
                }
            }
            _contexto.SaveChanges();
        }

        public bool VerifiqueAutenticidadeDoUsuario(Usuario obj)
        {
            return _DbSet.Where(x => x.Nome.ToLowerInvariant() == obj.Nome.ToLowerInvariant() && x.Senha == UtilitarioDeCriptografia.Criptografe(obj.Senha)).Any();
        }
    }
}
