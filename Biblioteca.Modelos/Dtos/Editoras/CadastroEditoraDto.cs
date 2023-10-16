using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation.Results;
using NPOI.SS.Util;

namespace Biblioteca.Negocio.Dtos.Editoras
{
    public class CadastroEditoraDto
    {

        private Conversor<CadastroEditoraDto, Editora> _Conversor;

        private InconsistenciaDeValidacao? inconsistenciaDeValidacao;

        public CadastroEditoraDto()
        {
            _Conversor = new Conversor<CadastroEditoraDto, Editora>();
        }

        public string Cnpj { get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set; }

        public bool IsValid()
        {
            inconsistenciaDeValidacao = new CadastroEditoraValidacao().ValidarCadastro(this);
            return inconsistenciaDeValidacao.EhValido();
        }
        public InconsistenciaDeValidacao RetornarInconsistencia() => inconsistenciaDeValidacao;

        public Editora ObtenhaEntidade() => _Conversor.ConvertaPara(this);
      
    }
}
