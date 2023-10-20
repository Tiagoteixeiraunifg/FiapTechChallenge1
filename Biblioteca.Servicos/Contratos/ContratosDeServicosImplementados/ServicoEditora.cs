using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Servicos.Contratos.Servicos;

namespace Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados
{
    public class ServicoEditora : EFRepositorioGenerico<Editora>, IServicoEditora
    {
        private readonly IEditoraRepositorio _editoraRepositorio;

        public ServicoEditora(ApplicationDbContext contexto, IEditoraRepositorio editoraRepositorio) : base(contexto)
        {
            _editoraRepositorio = editoraRepositorio;
        }

        public InconsistenciaDeValidacao Atualizar(AlterarEditoraDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            var editora = ObterPorId(dto.Id);

            if (editora is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

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

            if (editora is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

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
