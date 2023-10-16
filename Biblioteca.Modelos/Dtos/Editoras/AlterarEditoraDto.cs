
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;

namespace Biblioteca.Negocio.Dtos.Editoras
{
    public class AlterarEditoraDto 
    {

        private Conversor<AlterarEditoraDto, Editora> _Conversor;

        private InconsistenciaDeValidacao? inconsistenciaDeValidacao;

        public AlterarEditoraDto()
        {
            _Conversor = new Conversor<AlterarEditoraDto, Editora>();
        }

        public int Id { get; set; }
        public Guid Codigo { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }

        
        public bool IsValid()
        {
            inconsistenciaDeValidacao = new AlterarEditoraValidacao().ValidarAlteracao(this);
            return inconsistenciaDeValidacao.EhValido();
        }

        public InconsistenciaDeValidacao RetornarInconsistencia() => inconsistenciaDeValidacao;
        
        public Editora ObtenhaEntidade(Editora editora = null)
        {
            var retorno = _Conversor.ConvertaPara(this);

            if (editora is not null) retorno.DataCriacao = editora.DataCriacao;
            
            return retorno;
        }
    }
}
