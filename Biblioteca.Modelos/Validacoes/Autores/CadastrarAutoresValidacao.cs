using Biblioteca.Negocio.Dtos.Autores;
using Biblioteca.Negocio.Dtos.Editoras;
using Biblioteca.Negocio.Enumeradores.Validacoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Autores
{
    public class CadastrarAutoresValidacao : ValidadorAbstratro<CadastrarAutorDto>
    {
        public InconsistenciaDeValidacao ValidarCadastro(CadastrarAutorDto dados)
        {
            AssineRegrasCadastro();
            return base.Valide(dados);
        }

        private void AssineRegrasCadastro()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um Nome ");


            RuleFor(x => x.Telefone)
                .NotEmpty()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um telefone ");
        }
    }
}
