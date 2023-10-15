using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;

namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoEditora
    {
        IList<Editora> ObterTodos();

        Editora ObterPorId(int Id);

        InconsistenciaDeValidacao Cadastrar(CadastroEditoraDto dto);

        InconsistenciaDeValidacao Atualizar(AlterarEditoraDto dto);

        InconsistenciaDeValidacao Deletar(int Id);

    }
}
