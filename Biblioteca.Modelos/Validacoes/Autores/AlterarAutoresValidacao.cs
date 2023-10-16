using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Autores
{
    public class AlterarAutoresValidacao : ValidadorAbstratro<AlterarAutorDto>
    {
        public InconsistenciaDeValidacao ValidarAlteracao(AlterarAutorDto dados)
        {
            AssineRegrasAlteracao();
            return base.Valide(dados);
        }

        private void AssineRegrasAlteracao()
        {

            RuleFor(x => x.Id)
                 .NotEmpty()
                 .WithMessage("Deve ser informado um Id");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Deve ser informado um Nome ");


            RuleFor(x => x.Telefone)
                .NotEmpty()
                .WithMessage("Deve ser informado um telefone ");
        }
    }
}
