using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico.Interfaces;
using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Servicos.Validacoes.Livros
{
    public class ServicoLivroValidador : ValidadorAbstratro<Livro>
    {

        private ApplicationDbContext _Contexto;

        private ApplicationDbContext Contexto { 
            get 
            {
                return ApplicationDbContext.NovaInstancia();
            } 
        }

        public ServicoLivroValidador() 
        {

        }

        public InconsistenciaDeValidacaoTipado<Livro> ValideInicial(Livro dto) 
        {
            AssineRegrasItensObrigatorios();
            
            return base.ValideTipado(dto);
        }





        private void AssineRegrasItensObrigatorios() 
        {
            RuleFor(x => x)
                .Must(x => VerifiqueSeEditoraInformadoExiste(x.EditoraId))
                .When(x => x.EditoraId.PossuiValor())
                .SobrescrevaPropriedade("EditoraId")
                .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("A Editora informado não consta cadastrado no sistema.");

            RuleForEach(x => x.Autores).Cascade(CascadeMode.Continue).ChildRules(v => {

                v.RuleFor(x => x)
                 .Must(x => VerifiqueSeAutoresInformadosExistem(x.AutorId))
                 .When(x => x.AutorId.PossuiValor())
                 .SobrescrevaPropriedade("Autor")
                 .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                 .WithMessage("O Autor informada não consta cadastrada no sistema.");
            
            });

        }

        private bool VerifiqueSeEditoraInformadoExiste(int IdDaEditora) 
        {
            using (IRepositorioGenerico<Editora> servico = new EFRepositorioGenerico<Editora>(Contexto))
            {
                var ret = servico.ObtenhaDbSet().AsNoTracking().Any(x => x.Id == IdDaEditora);
                return ret;
            }
        }

        private bool VerifiqueSeAutoresInformadosExistem(int IdDoAutor) 
        {
            using (IRepositorioGenerico<Autor> servico = new EFRepositorioGenerico<Autor>(Contexto))
            {
                var ret = servico.ObtenhaDbSet().AsNoTracking().Any(x => x.Id == IdDoAutor);
                return ret;
            }
        }

    }
}
