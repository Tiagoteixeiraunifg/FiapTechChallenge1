using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Negocio.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes
{
    public class UsuarioRepositorioImpl : EFRepositorioGenerico<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorioImpl(ApplicationDbContext contexto) : base(contexto)
        {
            
        }

        public bool VerifiqueAutenticidadeDoUsuario(Usuario obj)
        {
            return _DbSet.Where(x => x.Nome.ToLowerInvariant() == obj.Nome.ToLowerInvariant() && x.Senha == UtilitarioDeCriptografia.Criptografe(obj.Senha)).Any();
        }
    }
}
