using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoAutor : EFRepositorioGenerico<Autor>, IServicoAutor
    {
        private readonly IAutorRepositorio _autoresRepositorio;

        public ServicoAutor(ApplicationDbContext contexto, IAutorRepositorio autoresRepositorio) : base(contexto)
        {
            _autoresRepositorio = autoresRepositorio;
        }

        public InconsistenciaDeValidacao Atualizar(AlterarAutorDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            var autor = ObterPorId(dto.Id);

            if (autor is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

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

            if (autor is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            //Verificar antes de deletar o autor se ele não está associado a algum livro, por que conflito com relacionamento do banco.
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
