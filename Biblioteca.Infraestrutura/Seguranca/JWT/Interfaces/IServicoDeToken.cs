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
        public string GerarToken(Usuario usuario);

        public Usuario ObtenhaUsuarioDoToken(string token);

        public bool ValidarToken(string token);

        public string RenoveToken(string token);
    }
}
