using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Negocio.Enumeradores.FichaEmprestimoAlunos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Negocio.Validacoes.FichaEmprestimoAlunos;

namespace Biblioteca.Servicos.Validacoes.EmprestimoAlunos
{
    public class ServicoValidacaoFichaEmprestimoAluno : FichaEmprestimoAlunoValidador
    {
        
        private ApplicationDbContext _Contexto;

        private ApplicationDbContext Contexto 
        {
            get 
            {
                _Contexto = ApplicationDbContext.NovaInstancia();

                return _Contexto;
            }
        }


        public ServicoValidacaoFichaEmprestimoAluno()
        {

        }


        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideFichaCadastro(FichaEmprestimoAluno dados)
        {
            AssineRegrasIniciaisCadastro(dados);
            AssineRegraDeQuantidadeDeLivrosDisponiveis(dados);
            AssineRegraDeEmprestimoEmAndamento(dados);

            return base.ValideTipado(dados);
        }

        public override InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideFinalizacaoFicha(FichaEmprestimoAluno dados) 
        {
            AssineRegrasIniciaisFinalizacao(dados);

            return base.ValideTipado(dados);
        } 



        #region CADASTRO DA FICHA

        private void AssineRegraDeQuantidadeDeLivrosDisponiveis(FichaEmprestimoAluno dados)
        {

            if (dados.FichaEmprestimoItens.PossuiValor() && dados.FichaEmprestimoItens.PossuiLinhas())
            {
                RuleForEach(x => x.FichaEmprestimoItens).Cascade(CascadeMode.Continue).ChildRules(v =>
                {

                    v.RuleFor(x => x)
                     .Must(x => LivroPossuiQuantidadePositiva(x.LivroId, x.Quantidade))
                     .When(x => x.Quantidade > 0)
                     .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                     .SobrescrevaPropriedade("Saldo do Livro")
                     .WithMessage("O Livro está sem saldo para emprestimo.");



                });

            }

        }

        private void AssineRegraDeEmprestimoEmAndamento(FichaEmprestimoAluno dados)
        {
            RuleFor(x => x)
                .Must(x => VerifiqueEmprestimoEmAndamentoDoAluno(x.AlunoId))
                .When(x => x.AlunoId.PossuiValor())
                .SobrescrevaPropriedade("EmprestimoEmAndamento")
                .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Não é possível emprestar enquanto não finalizar os Emprestimos em aberto do Aluno.");
        }

        private bool LivroPossuiQuantidadePositiva(int LivroId, decimal quantidadeSolicitada)
        {
            bool possui = false;
            decimal quantidadeTotalLivro = 0;
            decimal quantidadeJaEmprestada = 0;



            using (IRepositorioGenerico<Livro> repLivro = new EFRepositorioGenerico<Livro>(Contexto))
            {
                var livro = repLivro.ObtenhaDbSet().AsNoTracking().ToList();

                quantidadeTotalLivro = livro.PossuiValor() && livro.Any(x => x.Id == LivroId) ? livro.Where(x => x.Id == LivroId).First().QuantidadeEstoque : 0; 
            }


            using (IRepositorioGenerico<FichaEmprestimoAluno> repFicha = new EFRepositorioGenerico<FichaEmprestimoAluno>(Contexto))
            {
                var lista = repFicha.ObtenhaDbSet()
               .AsNoTracking()
               .Where(x => x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.NORMAL && x.FichaEmprestimoItens.Any(x => x.LivroId == LivroId))
               .Include(x => x.FichaEmprestimoItens)
               .ToList();

                if (lista.PossuiValor() && lista.PossuiLinhas())
                {
                    foreach (var item in lista)
                    {
                        quantidadeJaEmprestada += item.FichaEmprestimoItens.Where(x => x.StatusItem == FichaEmprestimoAlunoItensStatusEnum.A_ENTREGAR).Sum(x => x.Quantidade);
                    }
                }
            }


            possui = quantidadeTotalLivro > quantidadeJaEmprestada;

            if (possui) 
            {
                var saldo = quantidadeTotalLivro - quantidadeJaEmprestada;
                possui = saldo - quantidadeSolicitada > 0;
            }

            return possui;
        }

        private bool VerifiqueEmprestimoEmAndamentoDoAluno(int AlunoId)
        {

            using (IRepositorioGenerico<FichaEmprestimoAluno> repFicha = new EFRepositorioGenerico<FichaEmprestimoAluno>(Contexto))
            {
                return !repFicha.ObtenhaDbSet()
                .AsNoTracking()
                .Any(x => x.AlunoId == AlunoId
                && x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.NORMAL
                || x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.ATRASADO);
            }

            
        }


        #endregion



    }
}
