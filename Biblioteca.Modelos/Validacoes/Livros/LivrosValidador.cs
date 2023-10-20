using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Livros
{
    public class LivrosValidador : ValidadorAbstratro<Livro>
    {

        public LivrosValidador() { }


        public InconsistenciaDeValidacaoTipado<Livro> ValideCadastro(Livro livro) 
        {
            AssineRegrasDeCadastro();
            return base.ValideTipado(livro);
        }


        public InconsistenciaDeValidacaoTipado<Livro> ValideAtualizacao(Livro livro)
        {
            AssineRegrasDeCadastro();
            AssineRegrasDeAtualizacao();
            return base.ValideTipado(livro);
        }



        private void AssineRegrasDeCadastro() 
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve incluir um título ao Livro");
            
            RuleFor(x => x.SubTitulo)
                .NotEmpty()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve incluir um sub título ao Livro");
            
            RuleFor(x => x.QuantidadeEstoque)
                .NotEmpty()
                .NotEqual(0)
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve incluir um estoque de ao menos 1 quantidade.");

            RuleFor(x => x)
                .Must(x => x.Autores.PossuiLinhas())
                .When(x => x.Autores.PossuiValor())
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado pelo menos um autor para o livro");

            RuleFor(x => x)
               .Must(x => x.EditoraId != 0)
               .Must(x => x.EditoraId.PossuiValor())
               .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
               .WithMessage("Deve ser informado um código de Editora válido");

        }

        private void AssineRegrasDeAtualizacao()
        {

            RuleFor(x => x.DataAtualizacao)
                .GreaterThan(DateTime.Now)
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.ADVERTENCIA)
                .WithMessage("A Data de atualização deve ser a data de hoje e ser menor que a data de cadastro");

        }
    }
}
