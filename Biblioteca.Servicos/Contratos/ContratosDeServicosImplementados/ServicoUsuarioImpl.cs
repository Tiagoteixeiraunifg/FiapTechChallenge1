using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Criptografia;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Servicos.Contratos.Servicos;


namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoUsuarioImpl : EFRepositorioGenerico<Usuario>, IServicoUsuario, IUsuarioRepositorio
    {
        public ServicoUsuarioImpl(ApplicationDbContext contexto) : base(contexto)
        {
            base._contexto = contexto;
        }

        public Usuario AtualizeUsuario(Usuario obj)
        {
            return base.Altere(obj);
        }

        public bool AutentiqueUsuario(Usuario obj)
        {
            return base.ObtenhaTodos().Where(x => x.Nome.ToLowerInvariant() == obj.Nome.ToLowerInvariant() && x.Senha == UtilitarioDeCriptografia.Criptografe(obj.Senha)).Any();
        }

        public Usuario Cadastrar(Usuario obj)
        {
            return base.Cadastre(obj);
        }

        public void CadastrarUsuarios(IList<Usuario> usuarios)
        {
            if(!usuarios.PossuiValor() || !usuarios.PossuiLinhas()) 
            {
                return;
            }

            var usuarioscadastrados = base.ObtenhaTodos();

            if (usuarioscadastrados.PossuiValor() && usuarioscadastrados.PossuiLinhas()) return;

            foreach (var usuario in usuarios) 
            {
                base.Cadastre(usuario);
            }

        }


        public bool DeleteUsuario(int Id)
        {
            return base.Delete(Id);
        }

        public IList<Usuario> ObtenhaTodosUsuarios()
        {
            return base.ObtenhaTodos();
        }
    }
}
