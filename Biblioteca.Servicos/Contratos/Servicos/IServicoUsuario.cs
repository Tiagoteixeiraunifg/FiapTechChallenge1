using Biblioteca.Negocio.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoUsuario
    {
        Usuario Cadastrar(Usuario obj);

        IList<Usuario> ObtenhaTodosUsuarios();

        Usuario AtualizeUsuario(Usuario obj);

        bool DeleteUsuario(int Id);

        bool AutentiqueUsuario(Usuario obj);
    }
}
