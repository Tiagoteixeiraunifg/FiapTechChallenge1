using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoEditora : EFRepositorioGenerico<Editora>, IServicoEditora
    {
        private readonly IEditoraRepositorio _editoraRepositorio;
        private readonly ILivroRepositorio _livroRepositorio;
        public ServicoEditora(ApplicationDbContext contexto, IEditoraRepositorio editoraRepositorio, ILivroRepositorio livroRepositorio) : base(contexto)
        {
            _editoraRepositorio = editoraRepositorio;
            _livroRepositorio = livroRepositorio;
        }

        public InconsistenciaDeValidacao Atualizar(AlterarEditoraDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            var editora = ObterPorId(dto.Id);

            if (editora.PossuiValor()) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            editora = dto.ObtenhaEntidade(editora);

            editora.DataAtualizacao = DateTime.Now;

            _editoraRepositorio.Altere(editora);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Cadastrar(CadastroEditoraDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            var entidade = dto.ObtenhaEntidade();
            entidade.Codigo = Guid.NewGuid();
            entidade.DataCriacao = DateTime.Now;
            entidade.DataAtualizacao = DateTime.Now;

            _editoraRepositorio.Cadastre(entidade);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Deletar(int id)
        {
            var editora = ObterPorId(id);

            if (!editora.PossuiValor()) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            var livro = _livroRepositorio.ConsultarLivroPorIdEditar(id);

            if(livro.PossuiValor()) return new InconsistenciaDeValidacao { Mensagem = $"O livro: {livro.Titulo} está relacionado a está editora, ela não pode ser deletada" };

            _editoraRepositorio.Delete(editora.Id);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

 

        public Editora ObterPorId(int Id)
        {
            return _editoraRepositorio.ObtenhaPorId(Id);
        }

        public IList<Editora> ObterTodos()
        {
            return _editoraRepositorio.ObtenhaTodos();
        }
    }
}
