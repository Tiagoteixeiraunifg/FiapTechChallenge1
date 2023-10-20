using Biblioteca.Negocio.Dtos.Usuarios;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.Validacoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Usuarios
{
    public class AutenticacaoValidador : ValidadorAbstratro<Usuario>
    {

        public AutenticacaoValidador()
        {
            
        }

        public InconsistenciaDeValidacao ValideAutenticacao(Usuario dados)
        {
            AssineRegrasDeAutenticacao(dados);

            return  base.Valide(dados);
        }


        public InconsistenciaDeValidacao ValideCadastro(Usuario dados)
        {
            AssineRegrasDeAutenticacao(dados);
            AssineRegrasDeCadastro(dados);

            return base.Valide(dados);
        }

        private void AssineRegrasDeAutenticacao(Usuario dados) 
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .NotNull()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Necessário Informar o Nome do Usuário");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .NotNull()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Necessaário Informar a Senha do Usuário");

        }

        private void AssineRegrasDeCadastro(Usuario dados) 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Email do Usuário Não Informado.");
        }

    }
}
