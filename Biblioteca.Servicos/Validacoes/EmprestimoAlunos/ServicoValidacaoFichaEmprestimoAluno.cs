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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Servicos.Validacoes.Livros;

namespace Biblioteca.Servicos.Validacoes.EmprestimoAlunos
{
    public class ServicoValidacaoFichaEmprestimoAluno : ValidadorAbstratro<FichaEmprestimoAluno>
    {
        private ApplicationDbContext _Contexto;

        public ServicoValidacaoFichaEmprestimoAluno()
        {
            _Contexto = ApplicationDbContext.Instancia();
        }
        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideFichaCadastro(FichaEmprestimoAluno dados) 
        {
            AssineRegraDeQuantidadeDeLivrosDisponiveis(dados);
            AssineRegraDeEmprestimoEmAndamento(dados);
            
            return base.ValideTipado(dados);
        }


        private void AssineRegraDeQuantidadeDeLivrosDisponiveis(FichaEmprestimoAluno dados)
        {

            if (dados.FichaEmprestimoItens.PossuiValor() && dados.FichaEmprestimoItens.PossuiLinhas())
            {
                RuleForEach(x => x.FichaEmprestimoItens).Cascade(CascadeMode.Continue).ChildRules(v =>
                {

                    v.RuleFor(x => x)
                    .Must(x => LivroPossuiQuantidadePositiva(x.LivroId))
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
        private bool LivroPossuiQuantidadePositiva(int LivroId) 
        {
            bool possui = false;
            decimal quantidadeTotalLivro = 0;
            decimal quantidadeJaEmprestada = 0;
            


            using (IRepositorioGenerico<Livro> repLivro = new EFRepositorioGenerico<Livro>(_Contexto))
            {
                quantidadeTotalLivro = repLivro.ObtenhaDbSet().AsNoTracking().Where(x => x.Id == LivroId).FirstOrDefault().QuantidadeEstoque;
            }


            using (IRepositorioGenerico<FichaEmprestimoAluno> repEmprestimo = new EFRepositorioGenerico<FichaEmprestimoAluno>(_Contexto))
            {
                var lista = repEmprestimo.ObtenhaDbSet()
                    .AsNoTracking()
                    .Where(x => x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.NORMAL && x.FichaEmprestimoItens.Any(x => x.LivroId == LivroId))
                    .Include(x => x.FichaEmprestimoItens)
                    .ToList();
                
                foreach (var item in lista) 
                {
                    quantidadeJaEmprestada += item.FichaEmprestimoItens.Sum(x => x.Quantidade);
                }
            }

            possui = quantidadeTotalLivro > quantidadeJaEmprestada;

            return possui;
        }

        private bool VerifiqueEmprestimoEmAndamentoDoAluno(int AlunoId) 
        {
            using (IRepositorioGenerico<FichaEmprestimoAluno> repEmprestimo = new EFRepositorioGenerico<FichaEmprestimoAluno>(_Contexto))
            {
                return !repEmprestimo.ObtenhaDbSet()
                    .AsNoTracking()
                    .Any(x => x.AlunoId == AlunoId 
                    && x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.NORMAL 
                    || x.StatusEmprestimo == FichaEmprestimoAlunoStatusEnum.ATRASADO);
            }

        }

    }
}
