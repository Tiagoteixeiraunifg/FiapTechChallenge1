
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
    public class UsuarioDto
    {
        private Conversor<UsuarioDto, Usuario> _Conversor;
        public UsuarioDto()
        {
            _Conversor = new Conversor<UsuarioDto, Usuario>();
        }

        public int Id { get; set; }

        public Guid Codigo { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public UsuarioPermissaoEnum Permissao { get; set; }


        public Usuario ObtenhaEntidade() 
        {
            return _Conversor.ConvertaPara(this);
        }
    }
}
