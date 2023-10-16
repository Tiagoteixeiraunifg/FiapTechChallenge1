using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.Autores;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;


namespace Biblioteca.Negocio.Dtos.Autores
{
    public class AlterarAutorDto
    {
        private Conversor<AlterarAutorDto, Autor> _Conversor;

        private InconsistenciaDeValidacao? inconsistenciaDeValidacao;

         public AlterarAutorDto()
        {
            _Conversor = new Conversor<AlterarAutorDto, Autor>();
        }

        public int Id { get; set; }
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }

        public bool IsValid()
        {
            inconsistenciaDeValidacao = new AlterarAutoresValidacao().ValidarAlteracao(this);
            return inconsistenciaDeValidacao.EhValido();
        }
        public InconsistenciaDeValidacao RetornarInconsistencia() => inconsistenciaDeValidacao;

        public Autor ObtenhaEntidade(Autor autor = null)
        {
            var retorno = _Conversor.ConvertaPara(this);

            if (autor is not null) retorno.DataCriacao = autor.DataCriacao;

            return retorno;
        }
    }
}
