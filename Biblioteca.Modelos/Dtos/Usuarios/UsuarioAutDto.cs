using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Negocio.Utilidades.Conversores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Dtos.Usuarios
{
    public class UsuarioAutDto
    {
        private Conversor<UsuarioAutDto, Usuario> _Conversor;
        public UsuarioAutDto()
        {
            _Conversor = new Conversor<UsuarioAutDto, Usuario>();
        }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public Usuario ObtenhaEntidade() => _Conversor.ConvertaPara(this);
    }
}
