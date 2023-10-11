using Biblioteca.Negocio.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Seguranca.JWT.Interfaces
{
    public interface IServicoDeToken
    {
        string GerarToken(Usuario usuario);

        bool ValidarToken(string token);

        string RenoveToken(string token);
    }
}
