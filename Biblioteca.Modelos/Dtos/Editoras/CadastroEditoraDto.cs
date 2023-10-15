using Biblioteca.Negocio.Validacoes.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation.Results;
using NPOI.SS.Util;

namespace Biblioteca.Negocio.Dtos.Editoras
{
    public class CadastroEditoraDto 
    {  

        public CadastroEditoraDto() {
                
        }

        public string Cnpj { get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set; }
        
        private InconsistenciaDeValidacao? inconsistenciaDeValidacao { get; set; }
        public bool IsValid()
        {
            inconsistenciaDeValidacao = new CadastroEditoraValidator().ValidarCadastro(this); 
           return inconsistenciaDeValidacao.EhValido();
        }
        public InconsistenciaDeValidacao RetornarInconsistencia()
        {
            return inconsistenciaDeValidacao;
        }
    }
}
