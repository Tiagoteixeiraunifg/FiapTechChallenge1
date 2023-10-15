

using Biblioteca.Negocio.Validacoes.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;

namespace Biblioteca.Negocio.Dtos.Editoras
{
    public class AlterarEditoraDto 
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }

        private InconsistenciaDeValidacao? inconsistenciaDeValidacao { get; set; }
        public bool IsValid()
        {
            inconsistenciaDeValidacao = new AlterarEditoraValidator().ValidarAlteracao(this);
            return inconsistenciaDeValidacao.EhValido();
        }
        public InconsistenciaDeValidacao RetornarInconsistencia()
        {
            return inconsistenciaDeValidacao;
        }
    }
}
