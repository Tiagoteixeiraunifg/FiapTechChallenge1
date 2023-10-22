using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoAutor : EFRepositorioGenerico<Autor>, IServicoAutor
    {
        private readonly IAutorRepositorio _autoresRepositorio;
        private readonly ILivroAutoresRepositorio _livroAutoresRepositorio;
        public ServicoAutor(ApplicationDbContext contexto, IAutorRepositorio autoresRepositorio, ILivroAutoresRepositorio livroAutoresRepositorio) : base(contexto)
        {
            _autoresRepositorio = autoresRepositorio;
            _livroAutoresRepositorio = livroAutoresRepositorio;
        }

        public InconsistenciaDeValidacao Atualizar(AlterarAutorDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            var autor = ObterPorId(dto.Id);

            if (!autor.PossuiValor()) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            autor = dto.ObtenhaEntidade(autor);

            _autoresRepositorio.Altere(autor);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Cadastrar(CadastrarAutorDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            _autoresRepositorio.Cadastre(dto.ObtenhaEntidade());

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Deletar(int id)
        {
            var autor = ObterPorId(id);

            if (!autor.PossuiValor()) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            var livrosAutores = _livroAutoresRepositorio.ConsultarLivroAutoresPorIdAutor(id);

            if(livrosAutores.PossuiValor()) new InconsistenciaDeValidacao { Mensagem = $"Existem livros relacionados a este autor, ele não pode ser deletado" };

            _autoresRepositorio.Delete(autor.Id);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public Autor ObterPorId(int Id)
        {
            return _autoresRepositorio.ObtenhaPorId(Id);
        }

        public IList<Autor> ObterTodos()
        {
            return _autoresRepositorio.ObtenhaTodos();
        }
    }
}
