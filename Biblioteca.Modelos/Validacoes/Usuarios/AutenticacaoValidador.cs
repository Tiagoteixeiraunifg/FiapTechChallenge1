using Biblioteca.Negocio.Dtos.Usuarios;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.Validacoes;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Usuarios
{
    public class AutenticacaoValidador : ValidadorAbstratro<Usuario>
    {

        public AutenticacaoValidador()
        {
            
        }

        public InconsistenciaDeValidacaoTipado<Usuario> ValideAutenticacao(Usuario dados)
        {
            AssineRegrasDeAutenticacao(dados);
            return  base.ValideTipado(dados);
        }


        public InconsistenciaDeValidacaoTipado<Usuario> ValideCadastro(Usuario dados)
        {
            AssineRegrasDeAutenticacao(dados);
            AssineRegrasDeCadastro(dados);
            AssineRegraDeEmailValido();
            return base.ValideTipado(dados);
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


            RuleFor(x => x)
                .Must(x => x.Senha.Length >= 5)
                .When(x => x.Senha.PossuiValor())
                .SobrescrevaPropriedade("Senha")
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("A senha deve conter mais que 5 digitos");

        }

        private void AssineRegraDeEmailValido() 
        {
            RuleFor(x => x)
                .Must(x => EmailEhValido(x.Email))
                .When(x => x.Email.PossuiValor())
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .SobrescrevaPropriedade("Email")
                .WithMessage("Deve ser informado um e-mail válido para o cadastro.");
        }

        private bool EmailEhValido(string email)
        {
            if (!email.PossuiValor()) return false;

            bool emailValido = false;

            //Expressão regular retirada de
            //https://msdn.microsoft.com/pt-br/library/01escwtf(v=vs.110).aspx
            string emailRegex = string.Format("{0}{1}",
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))",
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

            try
            {
                emailValido = Regex.IsMatch(email.ToLower(), emailRegex);
            }
            catch (RegexMatchTimeoutException)
            {
                emailValido = false;
            }

            return emailValido;

        }

    }
}
