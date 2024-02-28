using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Negocio.Validacoes.Usuarios;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Servicos.Validacoes.Usuarios
{
    public class ServicoUsuarioValidador : AutenticacaoValidador
    {


        private ApplicationDbContext _Contexto;

        private ApplicationDbContext Contexto
        {
            get
            {
                return ApplicationDbContext.NovaInstancia();
            }
        }

        public ServicoUsuarioValidador()
        {
        }

        public  InconsistenciaDeValidacaoTipado<Usuario> ValideCadastroImpeditivo(Usuario dados) 
        {

            ValideDuplicidadeDeCadastro(dados);

            return base.ValideTipado(dados);    
        }


        private void ValideDuplicidadeDeCadastro(Usuario dados) 
        {

            RuleFor(x => x.Nome)
                .Must(x => !ExisteNomeCadastrado(dados.Nome))
                .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Nome Já Cadastrado no Sistema");

            RuleFor(x => x.Email)
                .Must(x => !ExisteEmailJaCadastrado(dados.Email))
                .TipoValidacao(Negocio.Enumeradores.Validacoes.TipoValidacaoEnum.IMPEDITIVA)
                .WithMessage("Já existe um cadastro com esse E-Mail");
        }


        private bool ExisteEmailJaCadastrado(string email) 
        {
            bool existe = false;

            using (IUsuarioRepositorio servico = new UsuarioRepositorioImpl(Contexto))
            {
                var lista = servico.ObtenhaDbSet().AsNoTracking().ToList();

                existe = lista.Any(x => x.Email == email);
            }

            return existe;
        }

        private bool ExisteNomeCadastrado(string nome) 
        {
            bool existe = false;
            
            using (IUsuarioRepositorio servico = new UsuarioRepositorioImpl(Contexto))
            {
                var lista = servico.ObtenhaDbSet().AsNoTracking().ToList();

                existe = lista.Any(x => x.Nome == nome);
            }
                

            return existe;
        }


    }
}
