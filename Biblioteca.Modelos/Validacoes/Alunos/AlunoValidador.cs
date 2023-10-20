using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Enumeradores.Validacoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;


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
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um Nome");

            RuleFor(x => x.Telefone)
                .NotNull()
                .NotEmpty()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um Telefone");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .TipoValidacao(TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Deve ser informado um email");
        }

    }
}
