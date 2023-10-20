using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.Editoras;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Enumeradores.Livros;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using Biblioteca.Negocio.Validacoes.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Dtos.Livros
{
    public class LivroDto : EntidadeBase
    {
        private Conversor<LivroDto, Livro> _Conversor;

        private InconsistenciaDeValidacaoTipado<Livro> _Inconsistencias;

        public LivroDto()
        {
            _Conversor = new Conversor<LivroDto, Livro>();
        }

        public LivrosTipoOperacaoDeDadosEnum TipoOperacaoDeDadosEnum { private get; set; }
       
        public Guid Codigo {  get; set; }

        public string Titulo { get; set; }

        public string SubTitulo { get; set; }

        public decimal QuantidadeEstoque { get; set; }

        public StatusLivroEnum Status { get; set; }

        public IList<LivroAutores> Autores { get; set; }

        public int EditoraId { get; set; }

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

        public Livro ObtenhaEntidade() => _Conversor.ConvertaPara(this);

    }
}
