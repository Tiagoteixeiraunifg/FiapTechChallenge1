using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.Alunos
{
    public class AlunoValidador : ValidadorAbstratro<Aluno>
    {
        public AlunoValidador() 
        {
        }

        public InconsistenciaDeValidacao ValideAtualizacaoDeAluno(Aluno dados) 
        {

           AssineRegrasDeCadastro();
           return  base.Valide(dados);
        }

        public InconsistenciaDeValidacao ValideCadastroDeAluno(Aluno dados)
        {


            AssineRegrasDeCadastro();
            return base.Valide(dados);
        }


        private void AssineRegrasDeCadastro() 
        {

            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage("Deve ser informado um Nome");

            RuleFor(x => x.Telefone)
                .NotNull()
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage("Deve ser informado um Telefone");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage("Deve ser informado um email");
        }

    }
}
