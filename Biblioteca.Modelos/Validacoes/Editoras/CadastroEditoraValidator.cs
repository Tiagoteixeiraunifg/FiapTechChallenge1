using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;


namespace Biblioteca.Negocio.Validacoes.Editoras
{
    public class CadastroEditoraValidator : ValidadorAbstratro<CadastroEditoraDto>
    {

        public InconsistenciaDeValidacao ValidarCadastro(CadastroEditoraDto dados)
        {
            AssineRegrasCadastro();
            return base.Valide(dados);
        }

        private void AssineRegrasCadastro()
        {

            RuleFor(x => x.Cnpj)
                 .NotEmpty()
                 .WithMessage("Deve ser informado um Cnpj ");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Deve ser informado um Nome ");


            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithMessage("Deve ser informado uma cidade ");
        }  
        
    }
}
