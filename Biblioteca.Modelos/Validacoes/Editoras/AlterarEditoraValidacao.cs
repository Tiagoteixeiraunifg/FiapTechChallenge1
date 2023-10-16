using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;

namespace Biblioteca.Negocio.Validacoes.Editoras
{
    public class AlterarEditoraValidacao : ValidadorAbstratro<AlterarEditoraDto>
    {

        public InconsistenciaDeValidacao ValidarAlteracao(AlterarEditoraDto dados)
        {
            AssineRegrasAlteracao();
            return base.Valide(dados);
        }

        private void AssineRegrasAlteracao()
        {

            RuleFor(x => x.Id)
                 .NotEmpty()
                 .WithMessage("Deve ser informado um Id");

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
