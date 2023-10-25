using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FichaEmprestimoAlunos
{
    public class FichaEmprestimoAlunoValidador : ValidadorAbstratro<FichaEmprestimoAluno>
    {

        public FichaEmprestimoAlunoValidador()
        {
            
        }


        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideCadastroFicha(FichaEmprestimoAluno dados) 
        {
            AssineRegrasCamposObrigatorios(dados);
            AssineRegrasDeCadastroDaFicha();
            return base.ValideTipado(dados);
        }

        public virtual InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideFinalizacaoFicha(FichaEmprestimoAluno dados)
        {
            AssineRegrasCamposObrigatorios(dados);
            AssineRegraDeFinalizacaoDaFicha(dados);
            
            return base.ValideTipado(dados);
        }

        protected void AssineRegrasIniciaisCadastro(FichaEmprestimoAluno dados) 
        {
            AssineRegrasCamposObrigatorios(dados);
            AssineRegrasDeCadastroDaFicha();
        }


        protected void AssineRegrasIniciaisFinalizacao(FichaEmprestimoAluno dados)
        {
            AssineRegrasCamposObrigatorios(dados);
            AssineRegraDeFinalizacaoDaFicha(dados);
        }

        private void AssineRegrasCamposObrigatorios(FichaEmprestimoAluno dados) 
        {

            RuleFor(x => x.AlunoId)
                .NotNull()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado o aluno");

            
            
            RuleFor(x => x.UsuarioId)
                .NotNull()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado o usuário logado no sistema.");

           
            
            RuleFor(x => x.DataCriacao)
                .NotEmpty()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado a data de criação.");
            
            
            RuleFor(x => x.FichaEmprestimoItens)
                .NotNull()
                .NotEmpty()
                .SobrescrevaPropriedade("ItensDaFichaDoAluno")
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado os itens da ficha.");

           
            RuleFor(x => x.Codigo)
                .NotNull()
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um código do tipo Guid");


        }
        private void AssineRegrasDeCadastroDaFicha()
        {

            RuleFor(x => x)
                .Must(x => x.StatusEmprestimo == Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoStatusEnum.NORMAL)
                .When(x => x.PossuiValor())
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .SobrescrevaPropriedade("Status Ficha")
                .WithMessage("Para cadastro de ficha o status deve ser informado como NORMAL");
        }


        private void AssineRegraDeFinalizacaoDaFicha(FichaEmprestimoAluno dados) 
        {

            RuleFor(x => x.DataEntregaEmprestimo.Date)
                .NotEqual(x => x.DataCriacao.Date)
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .SobrescrevaPropriedade("Data de Entrega")
                .WithMessage("A data da entrega tem de ser diferente da data de cadastro do livro.");

            RuleFor(x => x.DataEntregaEmprestimo.Date)
                .GreaterThan(x => x.DataCriacao.Date)
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .SobrescrevaPropriedade("Data de Entrega")
                .WithMessage("A data da entrega tem de ser maior que a data de cadastro.");


            RuleFor(x => x.StatusEmprestimo)
                .Equal(Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoStatusEnum.ENTREGUE)
                .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Para finalização da ficha deve ser informado o status como ENTREGUE");

            RuleForEach(x => x.FichaEmprestimoItens).Cascade(CascadeMode.Continue).ChildRules(v => 
            {

                v.RuleFor(x => x.DataStatusItem.Date)
                    .GreaterThan(x => dados.DataCriacao.Date)
                    .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                    .WithMessage("A data do status do item deve ser maior que a data de criação da ficha");


                v.RuleFor(x => x.StatusItem)
                    .NotNull()
                    .Equal(Enumeradores.FichaEmprestimoAlunos.FichaEmprestimoAlunoItensStatusEnum.ENTREGUE)
                    .TipoValidacao(Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                    .WithMessage("Para finalização de ficha deve informar o item como ENTREGUE");

            });

        }

    }
}
