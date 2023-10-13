
using Biblioteca.Negocio.Dtos.Usuarios;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Biblioteca.Negocio.Utilidades.Conversores;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Entidades.Usuarios
{
    public class Usuario : EntidadeBase
    {

        private Conversor<Usuario, UsuarioDto> _Conversor;

        public Usuario()
        {
            _Conversor = new Conversor<Usuario, UsuarioDto>();
        }

        public Guid Codigo { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public UsuarioPermissaoEnum Permissao { get; set; }

        public UsuarioDto ObtenhaDto() 
        {
            return _Conversor.ConvertaPara(this);
        }
    }
}
