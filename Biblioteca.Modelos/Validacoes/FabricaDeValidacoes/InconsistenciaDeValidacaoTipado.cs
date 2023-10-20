using Biblioteca.Negocio.Enumeradores.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FabricaDeValidacoes
{
    public class InconsistenciaDeValidacaoTipado<T> 
    {
        public TipoValidacaoEnum TipoValidacao { get; set; }
        public string? PropriedadeValidada { get; set; }
        public string? Mensagem { get; set; }
        public IList<InconsistenciaDeValidacao> listaDeInconsistencias { get; set; } = new List<InconsistenciaDeValidacao>();
        public IList<string> Mensagens { get; set; } = new List<string>();

        public object _RetornoServico { get; set; }

        public T ObtenhaRetornoServico() 
        {
            T convertido = (T)_RetornoServico;
            return convertido;
        }

        public bool EhValido()
        {
            return listaDeInconsistencias.Count == 0;
        }
    }
}
