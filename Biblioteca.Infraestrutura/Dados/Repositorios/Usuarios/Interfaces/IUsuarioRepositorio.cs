using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Negocio.Entidades.Usuarios;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioGenerico<Usuario>
    {
       void CadastrarUsuarios(IList<Usuario> usuarios);
    }
}
