using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.Autores;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;


namespace Biblioteca.Negocio.Dtos.Autores
{
    public class CadastrarAutorDto
    {
        private Conversor<CadastrarAutorDto, Autor> _Conversor;

        private InconsistenciaDeValidacao? inconsistenciaDeValidacao;


        public CadastrarAutorDto()
        {
            _Conversor = new Conversor<CadastrarAutorDto, Autor>();
        }

        public Guid Codigo { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public bool IsValid()
        {
            inconsistenciaDeValidacao = new CadastrarAutoresValidacao().ValidarCadastro(this);
            return inconsistenciaDeValidacao.EhValido();
        }
        public InconsistenciaDeValidacao RetornarInconsistencia() => inconsistenciaDeValidacao;

        public Autor ObtenhaEntidade() => _Conversor.ConvertaPara(this);
    }
}
