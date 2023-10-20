using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Enumeradores.Livros;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Utilidades.Extensoes;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Negocio.Validacoes.Livros;
using System.Text.Json.Serialization;

namespace Biblioteca.Negocio.Dtos.Livros
{
    public class LivroDto : EntidadeBase
    {
        private Conversor<LivroDto, Livro> _Conversor;

        private InconsistenciaDeValidacaoTipado<Livro> _Inconsistencias;

        public LivroDto()
        {
            _Conversor = new Conversor<LivroDto, Livro>();
            Autores = new List<LivroAutores>();
        }

        public LivrosTipoOperacaoDeDadosEnum TipoOperacaoDeDadosEnum { private get; set; }
       
        public Guid Codigo {  get; set; }

        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public decimal QuantidadeEstoque { get; set; }

        public StatusLivroEnum Status { get; set; }

        [JsonIgnore]
        public IList<LivroAutores> Autores { get; set; }

        public int EditoraId { get; set; }

        public IList<LivrosAutoresDto> AutoresDto { get; set; }


        public bool IsValid() 
        {

            switch (TipoOperacaoDeDadosEnum)
            {
                case LivrosTipoOperacaoDeDadosEnum.ALTERAR:
                    _Inconsistencias = new LivrosValidador().ValideAtualizacao(this.ObtenhaEntidade());
                    break;
                case LivrosTipoOperacaoDeDadosEnum.CADASTRAR:
                    _Inconsistencias = new LivrosValidador().ValideCadastro(this.ObtenhaEntidade());
                    break;
                case LivrosTipoOperacaoDeDadosEnum.DELETAR:
                    break;
                default:
                    break;
            }
            
            return _Inconsistencias.EhValido();
        }

        public InconsistenciaDeValidacaoTipado<Livro> RetornarInconsistencia() => _Inconsistencias;

        public Livro ObtenhaEntidade() 
        {
            CarregueListaDeAutoresPeloDto(); 
            return _Conversor.ConvertaPara(this);
        } 

        private void CarregueListaDeAutoresPeloDto() 
        {
            if (AutoresDto.PossuiValor() && AutoresDto.PossuiLinhas())
            {

                foreach (var item in AutoresDto)
                {
                    Autores.Add(
                        new LivroAutores()
                        {
                            Codigo = Guid.NewGuid(),
                            AutorId = item.AutorId,
                        }
                        );
                }
            }
        }

    }
}
