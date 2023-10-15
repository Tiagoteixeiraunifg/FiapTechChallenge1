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

            var Editora = ObterPorId(dto.Id);

            if (Editora is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            var retorno = Editora.Atualizar(dto);

            _editoraRepositorio.Altere(retorno);

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Cadastrar(CadastroEditoraDto dto)
        {
            if (!dto.IsValid()) return dto.RetornarInconsistencia();

            _editoraRepositorio.Cadastre(new Editora(dto));

            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public InconsistenciaDeValidacao Deletar(int Id)
        {
            var Editora = ObterPorId(Id);

            if(Editora is null) return new InconsistenciaDeValidacao { Mensagem = "Não registrado" };

            _editoraRepositorio.Delete(Editora.Id);
            
            return new InconsistenciaDeValidacao { Mensagem = "Sucesso" };
        }

        public Editora ObterPorId(int Id)
        {
          return _editoraRepositorio.ObtenhaPorId(Id);
        }

        public IList<Editora> ObterTodos()
        {
         return  _editoraRepositorio.ObtenhaTodos();
        }
    }
}
