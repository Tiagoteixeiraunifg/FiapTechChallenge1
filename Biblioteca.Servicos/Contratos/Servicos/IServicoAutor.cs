using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;

namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoAutor
    {
        IList<Autor> ObterTodos();

        Autor ObterPorId(int Id);

        InconsistenciaDeValidacao Cadastrar(CadastrarAutorDto dto);

        InconsistenciaDeValidacao Atualizar(AlterarAutorDto dto);

        InconsistenciaDeValidacao Deletar(int Id);
    }
}
